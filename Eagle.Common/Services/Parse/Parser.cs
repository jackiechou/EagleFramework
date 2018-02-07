using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace Eagle.Common.Services.Parse
{
    /// <remarks>
    /// Template Parser is simple parser has been written on C#.
    /// It allows setup variables and conditions block in template.
    /// Also you can use some of variable's modificators.
    ///
    ///     Author: Alexander Kleshevnikov
    ///     E-mail: seigo@icconline.com
    ///
    /// <example>There is the simpl example of template for html page:
    /// <code>
    /// <html>
    /// <head><title>##Title##</title></head>
    /// <body><h1>##Title:upper##</h1>
    /// ##If--IsRegisteredUser##
    /// Hello, ##UserName##!
    /// ##Else--IsRegisteredUser##
    /// Please sign in.
    /// ##EndIf--IsRegisteredUser##
    /// </body>
    /// </html>
    /// </code>
    /// To parse this template you can use the following code:
    /// <code>
    /// ...
    /// Hashtable Variables = new Hashtable();
    /// Variables.Add("Title", "Login In");
    /// Variables.Add("IsRegisteredUser", true);
    /// Variables.Add("UserName", "seigo");
    /// TemplateParser tpl = new TemplateParser("template.htm", Variables);
    /// tpl.ParseToFile("result.htm");
    /// ...
    /// </code>
    /// </example>
    /// </remarks>
    public class Parser
    {
        private string _strTemplateBlock;
        private Hashtable _hstValues;
        private Hashtable _errorMessage = new Hashtable();
        private string _parsedBlock;

        private Dictionary<string, Parser> _blocks = new Dictionary<string, Parser>();

        private string _variableTagBegin = "##";
        private string _variableTagEnd = "##";

        private string _modificatorTag = ":";
        private string _modificatorParamSep = ",";

        private string _conditionTagIfBegin = "##If--";
        private string _conditionTagIfEnd = "##";
        private string _conditionTagElseBegin = "##Else--";
        private string _conditionTagElseEnd = "##";
        private string _conditionTagEndIfBegin = "##EndIf--";
        private string _conditionTagEndIfEnd = "##";

        private string _blockTagBeginBegin = "##BlockBegin--";
        private string _blockTagBeginEnd = "##";
        private string _blockTagEndBegin = "##BlockEnd--";
        private string _blockTagEndEnd = "##";

        /// <value>Template block</value>
        public string TemplateBlock
        {
            get { return this._strTemplateBlock; }
            set
            {
                this._strTemplateBlock = value;
                ParseBlocks();
            }
        }

        /// <value>Template Variables</value>
        public Hashtable Variables
        {
            get { return this._hstValues; }
            set { this._hstValues = value; }
        }

        /// <value>Error Massage</value>
        public Hashtable ErrorMessage
        {
            get { return _errorMessage; }
        }

        /// <value>Blocks inside template</value>
        public Dictionary<string, Parser> Blocks
        {
            get { return _blocks; }
        }

        /// <summary>
        /// Creates a new instance of TemplateParser
        /// </summary>

        #region Contructors
        public Parser()
        {
            this._strTemplateBlock = "";
        }

        public Parser(string filePath)
        {
            ReadTemplateFromFile(filePath);
            ParseBlocks();
        }

        public Parser(Hashtable variables)
        {
            this._hstValues = variables;
        }

        public Parser(string filePath, Hashtable variables)
        {
            ReadTemplateFromFile(filePath);
            this._hstValues = variables;
            ParseBlocks();
        }
        #endregion

        /// <summary>
        /// Setup template from specified file
        /// </summary>
        /// <param name="filePath">Full phisical path to template file</param>
        public void SetTemplateFromFile(string filePath)
        {
            ReadTemplateFromFile(filePath);
        }

        /// <summary>
        /// Setup template as string block
        /// </summary>
        /// <param name="templateBlock">String template block</param>
        public void SetTemplate(string templateBlock)
        {
            this.TemplateBlock = templateBlock;
        }

        /// <summary>
        /// Parse template after setuping Template and Variables
        /// </summary>
        /// <returns>
        /// Parsed Block for Whole Template
        /// </returns>
        public string Parse()
        {
            ParseConditions();
            ParseVariables();
            return this._parsedBlock;
        }

        /// <summary>
        /// Parse Template Block
        /// </summary>
        /// <returns>
        /// Parsed Block for Specified BlockName
        /// </returns>
        public string ParseBlock(string blockName, Hashtable variables)
        {
            if (!this._blocks.ContainsKey(blockName))
            {
                throw new ArgumentException(String.Format("Could not find Block with Name '{0}'", blockName));
            }

            this._blocks[blockName].Variables = variables;
            return this._blocks[blockName].Parse();
        }

        /// <summary>
        /// Parse template and save result into specified file
        /// </summary>
        /// <param name="filePath">Full physical path to file</param>
        /// <param name="replaceIfExists">If true file which already exists
        /// will be replaced</param>
        /// <returns>True if new content has been written</returns>
        public bool ParseToFile(string filePath, bool replaceIfExists)
        {
            if (File.Exists(filePath) && !replaceIfExists)
            {
                return false;
            }
            else
            {
                StreamWriter sr = File.CreateText(filePath);
                sr.Write(Parse());
                sr.Close();
                return true;
            }
        }

        /// <summary>
        /// Read template content from specified file
        /// </summary>
        /// <param name="filePath">Full physical path to template file</param>
        private void ReadTemplateFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException("Template file does not exist.");
            }

            StreamReader reader = new StreamReader(filePath);
            this.TemplateBlock = reader.ReadToEnd();
            reader.Close();
        }

        /// <summary>
        /// Parse all blocks in template
        /// </summary>
        private void ParseBlocks()
        {
            //int idxPrevious = 0;
            int idxCurrent = 0;
            while ((idxCurrent = this._strTemplateBlock.IndexOf(this._blockTagBeginBegin, idxCurrent)) != -1)
            {
                string blockName;
                int idxBlockBeginBegin, idxBlockBeginEnd, idxBlockEndBegin;

                idxBlockBeginBegin = idxCurrent;
                idxCurrent += this._blockTagBeginBegin.Length;

                // Searching for BlockBeginEnd Index

                idxBlockBeginEnd = this._strTemplateBlock.IndexOf(this._blockTagBeginEnd, idxCurrent);
                if (idxBlockBeginEnd == -1) throw new Exception("Could not find BlockTagBeginEnd");

                // Getting Block Name

                blockName = this._strTemplateBlock.Substring(idxCurrent, (idxBlockBeginEnd - idxCurrent));
                idxCurrent = idxBlockBeginEnd + this._blockTagBeginEnd.Length;

                // Getting End of Block index

                string endBlockStatment = this._blockTagEndBegin + blockName + this._blockTagEndEnd;
                idxBlockEndBegin = this._strTemplateBlock.IndexOf(endBlockStatment, idxCurrent);
                if (idxBlockEndBegin == -1) throw new Exception("Could not find End of Block with name '" + blockName + "'");

                // Add Block to Dictionary

                Parser block = new Parser();
                block.TemplateBlock = this._strTemplateBlock.Substring(idxCurrent, (idxBlockEndBegin - idxCurrent));
                this._blocks.Add(blockName, block);

                // Remove Block Declaration From Template

                this._strTemplateBlock = this._strTemplateBlock.Remove(idxBlockBeginBegin, (idxBlockEndBegin - idxBlockBeginBegin));

                idxCurrent = idxBlockBeginBegin;
            }
        }

        /// <summary>
        /// Parse all conditions in template
        /// </summary>
        private void ParseConditions()
        {
            int idxPrevious = 0;
            int idxCurrent = 0;
            this._parsedBlock = "";
            while ((idxCurrent = this._strTemplateBlock.IndexOf(this._conditionTagIfBegin, idxCurrent)) != -1)
            {
                string varName;
                string trueBlock, falseBlock;
                string elseStatment, endIfStatment;
                int idxIfBegin, idxIfEnd, idxElseBegin, idxEndIfBegin;
                bool boolValue;

                idxIfBegin = idxCurrent;
                idxCurrent += this._conditionTagIfBegin.Length;

                // Searching for EndIf Index

                idxIfEnd = this._strTemplateBlock.IndexOf(this._conditionTagIfEnd, idxCurrent);
                if (idxIfEnd == -1) throw new Exception("Could not find ConditionTagIfEnd");

                // Getting Value Name

                varName = this._strTemplateBlock.Substring(idxCurrent, (idxIfEnd - idxCurrent));

                idxCurrent = idxIfEnd + this._conditionTagIfEnd.Length;

                // Compare ElseIf and EndIf Indexes

                elseStatment = this._conditionTagElseBegin + varName + this._conditionTagElseEnd;
                endIfStatment = this._conditionTagEndIfBegin + varName + this._conditionTagEndIfEnd;
                idxElseBegin = this._strTemplateBlock.IndexOf(elseStatment, idxCurrent);
                idxEndIfBegin = this._strTemplateBlock.IndexOf(endIfStatment, idxCurrent);
                if (idxElseBegin > idxEndIfBegin) throw new Exception("Condition Else Tag placed after Condition Tag EndIf for '" + varName + "'");

                // Getting True and False Condition Blocks

                if (idxElseBegin != -1)
                {
                    trueBlock = this._strTemplateBlock.Substring(idxCurrent, (idxElseBegin - idxCurrent));
                    falseBlock = this._strTemplateBlock.Substring((idxElseBegin + elseStatment.Length), (idxEndIfBegin - idxElseBegin - elseStatment.Length));
                }
                else
                {
                    trueBlock = this._strTemplateBlock.Substring(idxCurrent, (idxEndIfBegin - idxCurrent));
                    falseBlock = "";
                }

                // Parse Condition

                try
                {
                    boolValue = Convert.ToBoolean(this._hstValues[varName]);
                }
                catch
                {
                    boolValue = false;
                }

                string beforeBlock = this._strTemplateBlock.Substring(idxPrevious, (idxIfBegin - idxPrevious));

                if (this._hstValues.ContainsKey(varName) && boolValue)
                {
                    this._parsedBlock += beforeBlock + trueBlock.Trim();
                }
                else
                {
                    this._parsedBlock += beforeBlock + falseBlock.Trim();
                }

                idxCurrent = idxEndIfBegin + endIfStatment.Length;
                idxPrevious = idxCurrent;
            }
            this._parsedBlock += this._strTemplateBlock.Substring(idxPrevious);
        }

        /// <summary>
        /// Parse all variables in template
        /// </summary>
        private void ParseVariables()
        {
            int idxCurrent = 0;
            while ((idxCurrent = this._parsedBlock.IndexOf(this._variableTagBegin, idxCurrent)) != -1)
            {
                string varName, varValue;
                int idxVarTagEnd;

                idxVarTagEnd = this._parsedBlock.IndexOf(this._variableTagEnd, (idxCurrent + this._variableTagBegin.Length));
                if (idxVarTagEnd == -1) throw new Exception(String.Format("Index {0}: could not find Variable End Tag", idxCurrent));

                // Getting Variable Name

                varName = this._parsedBlock.Substring((idxCurrent + this._variableTagBegin.Length), (idxVarTagEnd - idxCurrent - this._variableTagBegin.Length));

                // Checking for Modificators

                string[] varParts = varName.Split(this._modificatorTag.ToCharArray());
                varName = varParts[0];

                // Getting Variable Value
                // If Variable doesn't exist in _hstValue then
                // Variable Value equal empty string

                // [added 6/6/2006] If variable is null than it will also has empty string

                varValue = String.Empty;
                if (this._hstValues.ContainsKey(varName) && this._hstValues[varName] != null)
                {
                    varValue = this._hstValues[varName].ToString();
                }

                // Apply All Modificators to Variable Value

                for (int i = 1; i < varParts.Length; i++)
                    this.ApplyModificator(ref varValue, varParts[i]);

                // Replace Variable in Template

                this._parsedBlock = this._parsedBlock.Substring(0, idxCurrent) + varValue + this._parsedBlock.Substring(idxVarTagEnd + this._variableTagEnd.Length);

                // Add Length of added value to Current index 
                // to prevent looking for variables in the added value
                // Fixed Date: April 5, 2006
                idxCurrent += varValue.Length;
            }
        }

        /// <summary>
        /// Method for applying modificators to variable value
        /// </summary>
        /// <param name="value">Variable value</param>
        /// <param name="modificator">Determination statment</param>
        private void ApplyModificator(ref string value, string modificator)
        {
            // Checking for parameters

            string strModificatorName = "";
            string strParameters = "";
            int idxStartBrackets, idxEndBrackets;
            if ((idxStartBrackets = modificator.IndexOf("(")) != -1)
            {
                idxEndBrackets = modificator.IndexOf(")", idxStartBrackets);
                if (idxEndBrackets == -1)
                {
                    throw new Exception("Incorrect modificator expression");
                }
                else
                {
                    strModificatorName = modificator.Substring(0, idxStartBrackets).ToUpper();
                    strParameters = modificator.Substring(idxStartBrackets + 1, (idxEndBrackets - idxStartBrackets - 1));
                }
            }
            else
            {
                strModificatorName = modificator.ToUpper();
            }
            string[] arrParameters = strParameters.Split(this._modificatorParamSep.ToCharArray());
            for (int i = 0; i < arrParameters.Length; i++)
                arrParameters[i] = arrParameters[i].Trim();

            try
            {
                Type typeModificator = Type.GetType("Template.Modificators." + strModificatorName);
                if (typeModificator.IsSubclassOf(Type.GetType("Template.Modificator")))
                {
                    Modificator objModificator = (Modificator)Activator.CreateInstance(typeModificator);
                    objModificator.Apply(ref value, arrParameters);
                }
            }
            catch
            {
                throw new Exception(String.Format("Could not find modificator '{0}'", strModificatorName));
            }
        }
    }
}

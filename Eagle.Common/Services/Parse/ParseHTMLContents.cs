using System;
using System.Collections.Generic;
using System.Collections;

namespace Eagle.Common.Services.Parse
{
    public class ParseHtmlContents
    {
        private string _strTemplateBlock;
        private Hashtable _hstValues;
        private readonly Hashtable _errorMessage = new Hashtable();
        private string _parsedBlock;

        private readonly Dictionary<string, Parser> _blocks = new Dictionary<string, Parser>();

        private string _variableTagBegin = "{";
        private string _variableTagEnd = "}";

        private string _modificatorTag = ":";
        private string _modificatorParamSep = ",";

        private string _conditionTagIfBegin = "}If--";
        private string _conditionTagIfEnd = "}";
        private string _conditionTagElseBegin = "{Else--";
        private string _conditionTagElseEnd = "}";
        private string _conditionTagEndIfBegin = "{EndIf--";
        private string _conditionTagEndIfEnd = "{";

        private string _blockTagBeginBegin = "{BlockBegin--";
        private string _blockTagBeginEnd = "}";
        private string _blockTagEndBegin = "{BlockEnd--";
        private string _blockTagEndEnd = "}";

        /// <value>Template block</value>
        public string TemplateBlock
        {
            get { return _strTemplateBlock; }
            set
            {
                _strTemplateBlock = value;
                ParseBlocks();
            }
        }

        /// <value>Template Variables</value>
        public Hashtable Variables
        {
            get { return _hstValues; }
            set { _hstValues = value; }
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
        public ParseHtmlContents()
        {
            _strTemplateBlock = "";
        }

        public ParseHtmlContents(string filePath)
        {
            ReadTemplateFromHtmlContents(filePath);
            ParseBlocks();
        }

        public ParseHtmlContents(Hashtable variables)
        {
            _hstValues = variables;
        }

        public ParseHtmlContents(string htmlContents, Hashtable variables)
        {
            ReadTemplateFromHtmlContents(htmlContents);
            _hstValues = variables;
            ParseBlocks();
        }
        #endregion

        /// <summary>
        /// Setup template from specified file
        /// </summary>
        /// <param name="htmlContents"></param>
        public void SetTemplateFromFile(string htmlContents)
        {
            ReadTemplateFromHtmlContents(htmlContents);
        }

        /// <summary>
        /// Setup template as string block
        /// </summary>
        /// <param name="templateBlock">String template block</param>
        public void SetTemplate(string templateBlock)
        {
            TemplateBlock = templateBlock;
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
            return _parsedBlock;
        }

        
        /// <summary>
        /// Parse Template Block
        /// </summary>
        /// <returns>
        /// Parsed Block for Specified BlockName
        /// </returns>
        public string ParseBlock(string blockName, Hashtable variables)
        {
            if (!_blocks.ContainsKey(blockName))
            {
                throw new ArgumentException($"Could not find Block with Name '{blockName}'");
            }

            _blocks[blockName].Variables = variables;
            return _blocks[blockName].Parse();
        }

        /// <summary>
        /// Read template content from specified file
        /// </summary>
        /// <param name="htmlContents"></param>
        private void ReadTemplateFromHtmlContents(string htmlContents)
        {
            if (!string.IsNullOrEmpty(htmlContents))
            {
                TemplateBlock = htmlContents;               
            }
        }

        /// <summary>
        /// Parse all blocks in template
        /// </summary>
        private void ParseBlocks()
        {
            //int idxPrevious = 0;
            int idxCurrent = 0;
            while ((idxCurrent = _strTemplateBlock.IndexOf(_blockTagBeginBegin, idxCurrent, StringComparison.Ordinal)) != -1)
            {
                int idxBlockBeginBegin, idxBlockBeginEnd, idxBlockEndBegin;

                idxBlockBeginBegin = idxCurrent;
                idxCurrent += _blockTagBeginBegin.Length;

                // Searching for BlockBeginEnd Index

                idxBlockBeginEnd = _strTemplateBlock.IndexOf(_blockTagBeginEnd, idxCurrent, StringComparison.Ordinal);
                if (idxBlockBeginEnd == -1) throw new Exception("Could not find BlockTagBeginEnd");

                // Getting Block Name

                var blockName = _strTemplateBlock.Substring(idxCurrent, (idxBlockBeginEnd - idxCurrent));
                idxCurrent = idxBlockBeginEnd + _blockTagBeginEnd.Length;

                // Getting End of Block index

                string endBlockStatment = _blockTagEndBegin + blockName + _blockTagEndEnd;
                idxBlockEndBegin = _strTemplateBlock.IndexOf(endBlockStatment, idxCurrent, StringComparison.Ordinal);
                if (idxBlockEndBegin == -1) throw new Exception("Could not find End of Block with name '" + blockName + "'");

                // Add Block to Dictionary

                Parser block = new Parser();
                block.TemplateBlock = _strTemplateBlock.Substring(idxCurrent, (idxBlockEndBegin - idxCurrent));
                _blocks.Add(blockName, block);

                // Remove Block Declaration From Template

                _strTemplateBlock = _strTemplateBlock.Remove(idxBlockBeginBegin, (idxBlockEndBegin - idxBlockBeginBegin));

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
            _parsedBlock = "";
            while ((idxCurrent = _strTemplateBlock.IndexOf(_conditionTagIfBegin, idxCurrent, StringComparison.Ordinal)) != -1)
            {
                string trueBlock, falseBlock;
                bool boolValue;

                var idxIfBegin = idxCurrent;
                idxCurrent += _conditionTagIfBegin.Length;

                // Searching for EndIf Index

                var idxIfEnd = _strTemplateBlock.IndexOf(_conditionTagIfEnd, idxCurrent, StringComparison.Ordinal);
                if (idxIfEnd == -1) throw new Exception("Could not find ConditionTagIfEnd");

                // Getting Value Name

                var varName = _strTemplateBlock.Substring(idxCurrent, (idxIfEnd - idxCurrent));

                idxCurrent = idxIfEnd + _conditionTagIfEnd.Length;

                // Compare ElseIf and EndIf Indexes

                var elseStatment = _conditionTagElseBegin + varName + _conditionTagElseEnd;
                var endIfStatment = _conditionTagEndIfBegin + varName + _conditionTagEndIfEnd;
                var idxElseBegin = _strTemplateBlock.IndexOf(elseStatment, idxCurrent, StringComparison.Ordinal);
                var idxEndIfBegin = _strTemplateBlock.IndexOf(endIfStatment, idxCurrent, StringComparison.Ordinal);
                if (idxElseBegin > idxEndIfBegin) throw new Exception("Condition Else Tag placed after Condition Tag EndIf for '" + varName + "'");

                // Getting True and False Condition Blocks

                if (idxElseBegin != -1)
                {
                    trueBlock = _strTemplateBlock.Substring(idxCurrent, (idxElseBegin - idxCurrent));
                    falseBlock = _strTemplateBlock.Substring((idxElseBegin + elseStatment.Length), (idxEndIfBegin - idxElseBegin - elseStatment.Length));
                }
                else
                {
                    trueBlock = _strTemplateBlock.Substring(idxCurrent, (idxEndIfBegin - idxCurrent));
                    falseBlock = "";
                }

                // Parse Condition

                try
                {
                    boolValue = Convert.ToBoolean(_hstValues[varName]);
                }
                catch
                {
                    boolValue = false;
                }

                string beforeBlock = _strTemplateBlock.Substring(idxPrevious, (idxIfBegin - idxPrevious));

                if (_hstValues.ContainsKey(varName) && boolValue)
                {
                    _parsedBlock += beforeBlock + trueBlock.Trim();
                }
                else
                {
                    _parsedBlock += beforeBlock + falseBlock.Trim();
                }

                idxCurrent = idxEndIfBegin + endIfStatment.Length;
                idxPrevious = idxCurrent;
            }
            _parsedBlock += _strTemplateBlock.Substring(idxPrevious);
        }

        /// <summary>
        /// Parse all variables in template
        /// </summary>
        private void ParseVariables()
        {
            int idxCurrent = 0;
            while ((idxCurrent = _parsedBlock.IndexOf(_variableTagBegin, idxCurrent, StringComparison.Ordinal)) != -1)
            {
                var idxVarTagEnd = _parsedBlock.IndexOf(_variableTagEnd, (idxCurrent + _variableTagBegin.Length), StringComparison.Ordinal);
                if (idxVarTagEnd == -1) throw new Exception($"Index {idxCurrent}: could not find Variable End Tag");

                // Getting Variable Name

                var varName = _parsedBlock.Substring((idxCurrent + _variableTagBegin.Length), (idxVarTagEnd - idxCurrent - _variableTagBegin.Length));

                // Checking for Modificators

                string[] varParts = varName.Split(_modificatorTag.ToCharArray());
                varName = varParts[0];

                // Getting Variable Value
                // If Variable doesn't exist in _hstValue then
                // Variable Value equal empty string

                // [added 6/6/2006] If variable is null than it will also has empty string

                var varValue = String.Empty;
                if (_hstValues.ContainsKey(varName) && _hstValues[varName] != null)
                {
                    varValue = _hstValues[varName].ToString();
                }

                // Apply All Modificators to Variable Value

                for (int i = 1; i < varParts.Length; i++)
                    ApplyModificator(ref varValue, varParts[i]);

                // Replace Variable in Template

                _parsedBlock = _parsedBlock.Substring(0, idxCurrent) + varValue + _parsedBlock.Substring(idxVarTagEnd + _variableTagEnd.Length);

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

            string strModificatorName;
            string strParameters = "";
            int idxStartBrackets;
            if ((idxStartBrackets = modificator.IndexOf("(", StringComparison.Ordinal)) != -1)
            {
                var idxEndBrackets = modificator.IndexOf(")", idxStartBrackets, StringComparison.Ordinal);
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
            string[] arrParameters = strParameters.Split(_modificatorParamSep.ToCharArray());
            for (int i = 0; i < arrParameters.Length; i++)
                arrParameters[i] = arrParameters[i].Trim();

            try
            {
                Type typeModificator = Type.GetType("Template.Modificators." + strModificatorName);
                if (typeModificator != null && typeModificator.IsSubclassOf(Type.GetType("Template.Modificator")))
                {
                    Modificator objModificator = (Modificator)Activator.CreateInstance(typeModificator);
                    objModificator.Apply(ref value, arrParameters);
                }
            }
            catch
            {
                throw new Exception($"Could not find modificator '{strModificatorName}'");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Entities.Common;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories
{
    public class CommonRepository : BaseRepository, ICommonRepository
    {
        public CommonRepository(IDataContext dataContext) : base(dataContext) { }

        public SelectList GeneratePeriod(string optionText = null, bool? isShowSelectText = true)
        {
            List<SelectListItem> lst = new List<SelectListItem> {
               new SelectListItem {
                   Text = "12:00 AM", Value = "28"
               },
               new SelectListItem {
                   Text = "12:15 AM", Value = "29"
               },
               new SelectListItem {
                   Text = "12:30 AM", Value = "30"
               },
               new SelectListItem {
                   Text = "12:45 AM", Value = "31"
               },
               new SelectListItem {
                   Text = "1:00 AM", Value = "32"
               },
               new SelectListItem {
                   Text = "1:15 AM", Value = "33"
               },
               new SelectListItem {
                   Text = "1:30 AM", Value = "34"
               },
               new SelectListItem {
                   Text = "1:45 AM", Value = "35"
               },
               new SelectListItem {
                   Text = "2:00 AM", Value = "36"
               },
               new SelectListItem {
                   Text = "2:15 AM", Value = "37"
               },
               new SelectListItem {
                   Text = "2:30 AM", Value = "38"
               },
               new SelectListItem {
                   Text = "2:45 AM", Value = "39"
               },
               new SelectListItem {
                   Text = "3:00 AM", Value = "40"
               },
               new SelectListItem {
                   Text = "3:15 AM", Value = "41"
               },
               new SelectListItem {
                   Text = "3:30 AM", Value = "42"
               },
               new SelectListItem {
                   Text = "3:45 AM", Value = "43"
               },
               new SelectListItem {
                   Text = "4:00 AM", Value = "44"
               },
               new SelectListItem {
                   Text = "4:15 AM", Value = "45"
               },
               new SelectListItem {
                   Text = "4:30 AM", Value = "46"
               },
               new SelectListItem {
                   Text = "4:45 AM", Value = "47"
               },
               new SelectListItem {
                   Text = "5:00 AM", Value = "48"
               },
               new SelectListItem {
                   Text = "5:15 AM", Value = "49"
               },
               new SelectListItem {
                   Text = "5:30 AM", Value = "50"
               },
               new SelectListItem {
                   Text = "5:45 AM", Value = "51"
               },
               new SelectListItem {
                   Text = "6:00 AM", Value = "52"
               },
               new SelectListItem {
                   Text = "6:15 AM", Value = "53"
               },
               new SelectListItem {
                   Text = "6:30 AM", Value = "54"
               },
               new SelectListItem {
                   Text = "6:45 AM", Value = "55"
               },
               new SelectListItem {
                   Text = "7:00 AM", Value = "56"
               },
               new SelectListItem {
                   Text = "7:15 AM", Value = "57"
               },
               new SelectListItem {
                   Text = "7:30 AM", Value = "58"
               },
               new SelectListItem {
                   Text = "7:45 AM", Value = "59"
               },
               new SelectListItem {
                   Text = "8:00 AM", Value = "60"
               },
               new SelectListItem {
                   Text = "8:15 AM", Value = "61"
               },
               new SelectListItem {
                   Text = "8:30 AM", Value = "62"
               },
               new SelectListItem {
                   Text = "8:45 AM", Value = "63"
               },
               new SelectListItem {
                   Text = "9:00 AM", Value = "64"
               },
               new SelectListItem {
                   Text = "9:15 AM", Value = "65"
               },
               new SelectListItem {
                   Text = "9:30 AM", Value = "66"
               },
               new SelectListItem {
                   Text = "9:45 AM", Value = "67"
               },
               new SelectListItem {
                   Text = "10:00 AM", Value = "68"
               },
               new SelectListItem {
                   Text = "10:15 AM", Value = "69"
               },
               new SelectListItem {
                   Text = "10:30 AM", Value = "70"
               },
               new SelectListItem {
                   Text = "10:45 AM", Value = "71"
               },
               new SelectListItem {
                   Text = "11:00 AM", Value = "72"
               },
               new SelectListItem {
                   Text = "11:15 AM", Value = "73"
               },
               new SelectListItem {
                   Text = "11:30 AM", Value = "74"
               },
               new SelectListItem {
                   Text = "11:45 AM", Value = "75"
               },
               new SelectListItem {
                   Text = "12:00 PM", Value = "76"
               },
               new SelectListItem {
                   Text = "12:15 PM", Value = "77"
               },
               new SelectListItem {
                   Text = "12:30 PM", Value = "78"
               },
               new SelectListItem {
                   Text = "12:45 PM", Value = "79"
               },
               new SelectListItem {
                   Text = "1:00 PM", Value = "80"
               },
               new SelectListItem {
                   Text = "1:15 PM", Value = "81"
               },
               new SelectListItem {
                   Text = "1:30 PM", Value = "82"
               },
               new SelectListItem {
                   Text = "1:45 PM", Value = "27"
               },
               new SelectListItem {
                   Text = "2:00 PM", Value = "83"
               },
               new SelectListItem {
                   Text = "2:15 PM", Value = "84"
               },
               new SelectListItem {
                   Text = "2:30 PM", Value = "85"
               },
               new SelectListItem {
                   Text = "2:45 PM", Value = "86"
               },
               new SelectListItem {
                   Text = "3:00 PM", Value = "87"
               },
               new SelectListItem {
                   Text = "3:15 PM", Value = "88"
               },
               new SelectListItem {
                   Text = "3:30 PM", Value = "89"
               },
               new SelectListItem {
                   Text = "3:45 PM", Value = "90"
               },
               new SelectListItem {
                   Text = "4:00 PM", Value = "91"
               },
               new SelectListItem {
                   Text = "4:15 PM", Value = "92"
               },
               new SelectListItem {
                   Text = "4:30 PM", Value = "93"
               },
               new SelectListItem {
                   Text = "4:45 PM", Value = "94"
               },
               new SelectListItem {
                   Text = "5:00 PM", Value = "95"
               },
               new SelectListItem {
                   Text = "5:15 PM", Value = "96"
               },
               new SelectListItem {
                   Text = "5:30 PM", Value = "97"
               },
               new SelectListItem {
                   Text = "5:45 PM", Value = "98"
               },
               new SelectListItem {
                   Text = "6:00 PM", Value = "99"
               },
               new SelectListItem {
                   Text = "6:15 PM", Value = "100"
               },
               new SelectListItem {
                   Text = "6:30 PM", Value = "101"
               },
               new SelectListItem {
                   Text = "6:45 PM", Value = "102"
               },
               new SelectListItem {
                   Text = "7:00 PM", Value = "103"
               },
               new SelectListItem {
                   Text = "7:15 PM", Value = "104"
               },
               new SelectListItem {
                   Text = "7:30 PM", Value = "105"
               },
               new SelectListItem {
                   Text = "7:45 PM", Value = "106"
               },
               new SelectListItem {
                   Text = "8:00 PM", Value = "107"
               },
               new SelectListItem {
                   Text = "8:15 PM", Value = "108"
               },
               new SelectListItem {
                   Text = "8:30 PM", Value = "109"
               },
               new SelectListItem {
                   Text = "8:45 PM", Value = "110"
               },
               new SelectListItem {
                   Text = "9:00 PM", Value = "111"
               },
               new SelectListItem {
                   Text = "9:15 PM", Value = "112"
               },
               new SelectListItem {
                   Text = "9:30 PM", Value = "113"
               },
               new SelectListItem {
                   Text = "9:45 PM", Value = "114"
               },
               new SelectListItem {
                   Text = "10:00 PM", Value = "115"
               },
               new SelectListItem {
                   Text = "10:15 PM", Value = "116"
               },
               new SelectListItem {
                   Text = "10:30 PM", Value = "117"
               },
               new SelectListItem {
                   Text = "10:45 PM", Value = "118"
               },
               new SelectListItem {
                   Text = "11:00 PM", Value = "119"
               },
               new SelectListItem {
                   Text = "11:15 PM", Value = "120"
               },
               new SelectListItem {
                   Text = "11:30 PM", Value = "121"
               },
               new SelectListItem {
                   Text = "11:45 PM", Value = "122"
               }
           };

            if (isShowSelectText != null && isShowSelectText == true)
            {
                lst.Insert(0, new SelectListItem
                {
                    Text = $"--- {(string.IsNullOrEmpty(optionText) ? LanguageResource.Select : optionText)} ---",
                    Value = ""
                });
            }

            return new SelectList(lst, "Value", "Text", null);
        }

        public MessageBoxModel ShowMessageBox(string messageType, string message)
        {
            string cssClass;
            switch (messageType)
            {
                case "warning":
                    cssClass = "alert alert-warning";
                    break;
                case "error":
                    cssClass = "alert alert-error";
                    break;
                case "success":
                    cssClass = "alert alert-success";
                    break;
                default:
                    cssClass = "alert alert-warning";
                    break;
            }

            return new MessageBoxModel
            {
                DisplayErrorMessage = true,
                PopupTitle = messageType.ToUpper(),
                CssClass = cssClass,
                Message = message
            };
        }
        public SelectList GetGenders(string selectedValue)
        {
            var list = new Dictionary<int?, string>
            {
                {0, LanguageResource.Male},
                {1, LanguageResource.Female},
                {-1, LanguageResource.NonSpecified}
            };
            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public SelectList GetGenderList(string selectedValue)
        {
            var list = new List<DropdownListItem>() {
                new DropdownListItem{Key = "0", Value = LanguageResource.Female},
                new DropdownListItem{Key = "1", Value =  LanguageResource.Male}
            };
            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public SelectList GetWorkTypes(string selectedValue)
        {
            var list = new Dictionary<int?, string>
            {
                {1, LanguageResource.FullTime},
                {2, LanguageResource.PartTime}
            };
            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public SelectList GetMonths(string languageCode, string selectedValue, bool? isShowSelectText = false)
        {
            var monthLst = Enumerable.Range(1, 12).Select(r => new SelectListItem
            {
                Text = (!string.IsNullOrEmpty(languageCode) && languageCode == LanguageType.Vietnamese) ? $"{LanguageResource.Month} {r}" : DateTimeFormatInfo.CurrentInfo.GetMonthName(r),
                Value = r.ToString()
            }).ToList();

            if (isShowSelectText != null && isShowSelectText == true)
            {
                monthLst.Insert(0, new SelectListItem{
                    Text = $"--- {LanguageResource.SelectMonth} ---",
                    Value = ""
                });
            }

            return new SelectList(monthLst, "Value", "Text", selectedValue);
        }

        public SelectList GetYears(int numberOfYears, string selectedValue, bool? isShowSelectText = false)
        {
            int startPoint = DateTime.Today.Year - numberOfYears;
            var list = Enumerable.Range(startPoint + 1, (DateTime.UtcNow.Year - startPoint)).Reverse().Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() }).ToList();
            if (list.Count == 0)
                list.Insert(0, new SelectListItem()
                {
                    Value = "-1",
                    Text =
                    $"-- {LanguageResource.None} --"
                });
            else
            {
                if (isShowSelectText != null && isShowSelectText == true)
                    list.Insert(0, new SelectListItem() { Value = "-1", Text = $"-- {LanguageResource.SelectYear} --"});
            }
            return new SelectList(list, "Value", "Text", selectedValue);
        }
        public SelectList PopulatePeriod(int? selectedValue = null, bool? isShowSelectText = true)
        {
            List<SelectListItem> lst = new List<SelectListItem> {
               new SelectListItem {
                   Text = "1:00 AM", Value = "60"
               },
               new SelectListItem {
                   Text = "1:15 AM", Value = "75"
               },
               new SelectListItem {
                   Text = "1:30 AM", Value = "90"
               },
               new SelectListItem {
                   Text = "1:45 AM", Value = "105"
               },
               new SelectListItem {
                   Text = "2:00 AM", Value = "120"
               },
               new SelectListItem {
                   Text = "2:15 AM", Value = "135"
               },
               new SelectListItem {
                   Text = "2:30 AM", Value = "150"
               },
               new SelectListItem {
                   Text = "2:45 AM", Value = "165"
               },
               new SelectListItem {
                   Text = "3:00 AM", Value = "180"
               },
               new SelectListItem {
                   Text = "3:15 AM", Value = "195"
               },
               new SelectListItem {
                   Text = "3:30 AM", Value = "210"
               },
               new SelectListItem {
                   Text = "3:45 AM", Value = "225"
               },
               new SelectListItem {
                   Text = "4:00 AM", Value = "240"
               },
               new SelectListItem {
                   Text = "4:15 AM", Value = "255"
               },
               new SelectListItem {
                   Text = "4:30 AM", Value = "270"
               },
               new SelectListItem {
                   Text = "4:45 AM", Value = "285"
               },
               new SelectListItem {
                   Text = "5:00 AM", Value = "300"
               },
               new SelectListItem {
                   Text = "5:15 AM", Value = "315"
               },
               new SelectListItem {
                   Text = "5:30 AM", Value = "330"
               },
               new SelectListItem {
                   Text = "5:45 AM", Value = "345"
               },
               new SelectListItem {
                   Text = "6:00 AM", Value = "360"
               },
               new SelectListItem {
                   Text = "6:15 AM", Value = "375"
               },
               new SelectListItem {
                   Text = "6:30 AM", Value = "390"
               },
               new SelectListItem {
                   Text = "6:45 AM", Value = "405"
               },
               new SelectListItem {
                   Text = "7:00 AM", Value = "420"
               },
               new SelectListItem {
                   Text = "7:15 AM", Value = "435"
               },
               new SelectListItem {
                   Text = "7:30 AM", Value = "450"
               },
               new SelectListItem {
                   Text = "7:45 AM", Value = "465"
               },
               new SelectListItem {
                   Text = "8:00 AM", Value = "480"
               },
               new SelectListItem {
                   Text = "8:15 AM", Value = "495"
               },
               new SelectListItem {
                   Text = "8:30 AM", Value = "510"
               },
               new SelectListItem {
                   Text = "8:45 AM", Value = "525"
               },
               new SelectListItem {
                   Text = "9:00 AM", Value = "540"
               },
               new SelectListItem {
                   Text = "9:15 AM", Value = "555"
               },
               new SelectListItem {
                   Text = "9:30 AM", Value = "570"
               },
               new SelectListItem {
                   Text = "9:45 AM", Value = "585"
               },
               new SelectListItem {
                   Text = "10:00 AM", Value = "600"
               },
               new SelectListItem {
                   Text = "10:15 AM", Value = "615"
               },
               new SelectListItem {
                   Text = "10:30 AM", Value = "630"
               },
               new SelectListItem {
                   Text = "10:45 AM", Value = "645"
               },
               new SelectListItem {
                   Text = "11:00 AM", Value = "660"
               },
               new SelectListItem {
                   Text = "11:15 AM", Value = "675"
               },
               new SelectListItem {
                   Text = "11:30 AM", Value = "690"
               },
               new SelectListItem {
                   Text = "11:45 AM", Value = "705"
               },
               new SelectListItem {
                   Text = "12:00 AM", Value = "720"
               },
               new SelectListItem {
                   Text = "12:15 AM", Value = "735"
               },
               new SelectListItem {
                   Text = "12:30 AM", Value = "750"
               },
               new SelectListItem {
                   Text = "12:45 AM", Value = "765"
               },
               new SelectListItem {
                   Text = "1:00 PM", Value = "780"
               },
               new SelectListItem {
                   Text = "1:15 PM", Value = "795"
               },
               new SelectListItem {
                   Text = "1:30 PM", Value = "810"
               },
               new SelectListItem {
                   Text = "1:45 PM", Value = "825"
               },
               new SelectListItem {
                   Text = "2:00 PM", Value = "840"
               },
               new SelectListItem {
                   Text = "2:15 PM", Value = "855"
               },
               new SelectListItem {
                   Text = "2:30 PM", Value = "870"
               },
               new SelectListItem {
                   Text = "2:45 PM", Value = "885"
               },
               new SelectListItem {
                   Text = "3:00 PM", Value = "900"
               },
               new SelectListItem {
                   Text = "3:15 PM", Value = "915"
               },
               new SelectListItem {
                   Text = "3:30 PM", Value = "930"
               },
               new SelectListItem {
                   Text = "3:45 PM", Value = "945"
               },
               new SelectListItem {
                   Text = "4:00 PM", Value = "960"
               },
               new SelectListItem {
                   Text = "4:15 PM", Value = "975"
               },
               new SelectListItem {
                   Text = "4:30 PM", Value = "980"
               },
               new SelectListItem {
                   Text = "4:45 PM", Value = "995"
               },
               new SelectListItem {
                   Text = "5:00 PM", Value = "1010"
               },
               new SelectListItem {
                   Text = "5:15 PM", Value = "1025"
               },
               new SelectListItem {
                   Text = "5:30 PM", Value = "1040"
               },
               new SelectListItem {
                   Text = "5:45 PM", Value = "1055"
               },
               new SelectListItem {
                   Text = "6:00 PM", Value = "1070"
               },
               new SelectListItem {
                   Text = "6:15 PM", Value = "1085"
               },
               new SelectListItem {
                   Text = "6:30 PM", Value = "1100"
               },
               new SelectListItem {
                   Text = "6:45 PM", Value = "1115"
               },
               new SelectListItem {
                   Text = "7:00 PM", Value = "1130"
               },
               new SelectListItem {
                   Text = "7:15 PM", Value = "1145"
               },
               new SelectListItem {
                   Text = "7:30 PM", Value = "1160"
               },
               new SelectListItem {
                   Text = "7:45 PM", Value = "1175"
               },
               new SelectListItem {
                   Text = "8:00 PM", Value = "1190"
               },
               new SelectListItem {
                   Text = "8:15 PM", Value = "1205"
               },
               new SelectListItem {
                   Text = "8:30 PM", Value = "1220"
               },
               new SelectListItem {
                   Text = "8:45 PM", Value = "1235"
               },
               new SelectListItem {
                   Text = "9:00 PM", Value = "1250"
               },
               new SelectListItem {
                   Text = "9:15 PM", Value = "1265"
               },
               new SelectListItem {
                   Text = "9:30 PM", Value = "1280"
               },
               new SelectListItem {
                   Text = "9:45 PM", Value = "1295"
               },
               new SelectListItem {
                   Text = "10:00 PM", Value = "1310"
               },
               new SelectListItem {
                   Text = "10:15 PM", Value = "1325"
               },
               new SelectListItem {
                   Text = "10:30 PM", Value = "1340"
               },
               new SelectListItem {
                   Text = "10:45 PM", Value = "1355"
               },
               new SelectListItem {
                   Text = "11:00 PM", Value = "1365"
               },
               new SelectListItem {
                   Text = "11:15 PM", Value = "1380"
               },
               new SelectListItem {
                   Text = "11:30 PM", Value = "1395"
               },
               new SelectListItem {
                   Text = "11:45 PM", Value = "1410"
               },
               new SelectListItem {
                   Text = "12:00 PM", Value = "1425"
               },
                new SelectListItem {
                   Text = "12:15 PM", Value = "1440"
               },
               new SelectListItem {
                   Text = "12:30 PM", Value = "1455"
               },
               new SelectListItem {
                   Text = "12:45 PM", Value = "1470"
               },
           };

            if (isShowSelectText != null && isShowSelectText == true)
            {
                lst.Insert(0, new SelectListItem
                {
                    Text = $"--- {LanguageResource.SelectPeriod } ---",
                    Value = ""
                });
            }

            return new SelectList(lst, "Value", "Text", selectedValue);
        }
        public SelectList PopulateArticleImageSizes(string selectedValue, bool isShowSelectText = false)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.LargeImage, Value = "600×600"},
                new SelectListItem {Text = LanguageResource.MediumImage, Value = "360×360"},
                new SelectListItem {Text = LanguageResource.SmallImage, Value = "200×200"}
            };
            if (isShowSelectText)
                lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public SelectList PopulateSummaryImageSizes(string selectedValue, bool isShowSelectText = false)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.LargeImage, Value = "150×150"},
                new SelectListItem {Text = LanguageResource.MediumImage, Value = "100×100"},
                new SelectListItem {Text = LanguageResource.SmallImage, Value = "50×50"}
            };
            if (isShowSelectText)
                lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public SelectList PopulatePermisionGroups(string selectedValue, bool isShowSelectText = false)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.SystemPage, Value = "SYSTEM_PAGE"},
                new SelectListItem {Text = LanguageResource.SystemModule, Value = "SYSTEM_MODULE"},
                new SelectListItem {Text = LanguageResource.SystemMenu, Value = "SYSTEM_MENU"}
            };
            if (isShowSelectText)
                lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public SelectList PopulateScopeTypeList(string selectedValue, bool isShowSelectText = false)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.Host, Value = "1"},
                new SelectListItem {Text = LanguageResource.Site, Value = "2"},
                new SelectListItem {Text = LanguageResource.Desktop, Value = "3"}
            };
            if (isShowSelectText)
                lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public SelectList PopulateActionList(string selectedValue)
        {
            //@Html.DropDownListFor(model=>model.ActionId, Model.ActionsList)
            IEnumerable<PermissionLevel> actionTypes = Enum.GetValues(typeof(PermissionLevel)).Cast<PermissionLevel>();
            List<SelectListItem> lst = (from action in actionTypes
                                        select new SelectListItem
                                        {
                                            Text = action.ToString(),
                                            Value = ((int)action).ToString()
                                        }).ToList();
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public SelectList GetAlignmentList(string selectedValue, bool isShowSelectText = false)
        {
            var lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.Left, Value = "Left"},
                new SelectListItem {Text = LanguageResource.Center, Value = "Center"},
                new SelectListItem {Text = LanguageResource.Right, Value = "Right"}
            };
            if (isShowSelectText)
                lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        #region STATUS Mode ============================================================================================================
        public SelectList GetEventStatus(string selectedValue)
        {
            var lst = new Dictionary<int?, string>
            {
                {1, LanguageResource.Open},
                {2, LanguageResource.Closed}
            };
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public SelectList GetObjectStatus(string selectedValue)
        {
            var lst = new List<DropdownListItem>() {
                new DropdownListItem{Key = "2", Value = "Sử dụng"},
                new DropdownListItem{Key = "3", Value = "Hư hỏng"},
                new DropdownListItem{Key = "4", Value = "Mất"},
                new DropdownListItem{Key = "5", Value = "Phó Bản"}
            };
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public SelectList GenerateThreeStatusModeList(string selectedValue, bool? isShowSelectText)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.Published, Value = "2"},
                new SelectListItem {Text = LanguageResource.Active, Value = "1"},
                new SelectListItem {Text = LanguageResource.InActive, Value = "0"}
            };
            if (isShowSelectText != null && isShowSelectText == true)
                lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public SelectList GenerateThreeStatusModeListWithOptionText(string optionText = null, bool? isShowSelectText = true)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.Published, Value = "2"},
                new SelectListItem {Text = LanguageResource.Active, Value = "1"},
                new SelectListItem {Text = LanguageResource.InActive, Value = "0"}
            };
            if (isShowSelectText != null && isShowSelectText == true)
                lst.Insert(0, new SelectListItem
                {
                    Text =
                    $"--- {(string.IsNullOrEmpty(optionText) ? LanguageResource.Select : optionText)} ---",
                    Value = ""
                });
            return new SelectList(lst, "Value", "Text", null);
        }

        public SelectList GenerateThreeStatusModeListWithOptionText(string selectedValue, string optionText, bool? isShowSelectText = true)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.Published, Value = "2"},
                new SelectListItem {Text = LanguageResource.Active, Value = "1"},
                new SelectListItem {Text = LanguageResource.InActive, Value = "0"}
            };
            if (isShowSelectText != null && isShowSelectText == true)
                lst.Insert(0, new SelectListItem
                {
                    Text =
                    $"--- {(string.IsNullOrEmpty(optionText) ? LanguageResource.Select : optionText)} ---",
                    Value = ""
                });

            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public SelectList GenerateTwoStatusModeList(string selectedValue, bool? isShowSelectText)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.Active, Value = "1"},
                new SelectListItem {Text = LanguageResource.InActive, Value = "0"}
            };
            if (isShowSelectText != null && isShowSelectText == true)
                lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            return new SelectList(lst, "Value", "Text", selectedValue);
        }
        public SelectList GenerateTwoStatusModeListWithOptionText(string optionText = null, bool? isShowSelectText = true)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.Active, Value = "1"},
                new SelectListItem {Text = LanguageResource.InActive, Value = "0"}
            };
            if (isShowSelectText != null && isShowSelectText == true)
                lst.Insert(0, new SelectListItem
                {
                    Text =
                    $"--- {(string.IsNullOrEmpty(optionText) ? LanguageResource.Select : optionText)} ---",
                    Value = ""
                });

            return new SelectList(lst, "Value", "Text", null);
        }
        public SelectList GenerateTwoStatusModeListWithOptionText(string selectedValue = null, string optionText = null, bool? isShowSelectText = true)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.Active, Value = "1"},
                new SelectListItem {Text = LanguageResource.InActive, Value = "0"}
            };
            if (isShowSelectText != null && isShowSelectText == true)
                lst.Insert(0, new SelectListItem
                {
                    Text =
                    $"--- {(string.IsNullOrEmpty(optionText) ? LanguageResource.Select : optionText)} ---",
                    Value = ""
                });

            return new SelectList(lst, "Value", "Text", selectedValue);
        }
        #endregion  Status Mode ===========================================================================================================

        public SelectList PopulateLinkTargets(string selectedValue, bool isShowSelectText = false)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.LoadInANewWindow, Value = "_blank"},
                new SelectListItem {Text = LanguageResource.LoadInTheSameFrameAsItWasClicked, Value = "_self"},
                new SelectListItem {Text = LanguageResource.LoadInTheParentFrameset, Value = "_parent"},
                new SelectListItem {Text = LanguageResource.LoadInTheFullBodyOfTheWindow, Value = "_top"}
            };
            if (isShowSelectText)
                lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public SelectList PopulateIsSecured(string selectedValue, bool isShowSelectText = false)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.IsAdmin, Value = "1"},
                new SelectListItem {Text = LanguageResource.IsDesktop, Value = "0"}
            };
            if (isShowSelectText)
                lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            return new SelectList(lst, "Value", "Text", selectedValue);
        }
        
        #region HR============================================================================================================

        public Dictionary<string, string> GetMonths()
        {
            var monthDict = new Dictionary<string, string>();
            for (var i = 1; i <= 12; i++)
            {
                monthDict.Add(i.ToString(), i.ToString());
            }
            return monthDict;
        }

        public List<DropdownListItem> GetPaymentTypes()
        {
            return new List<DropdownListItem>() {
                new DropdownListItem{Key = "1", Value = "Chuyển khoản" , Desc = ""},
                new DropdownListItem{Key = "2", Value = "Tiền mặt" , Desc = ""},
            };
        }
        public List<DropdownListItem> GetMoneyTypes()
        {
            return new List<DropdownListItem>() {
                new DropdownListItem{Key = "1", Value = "VND" , Desc = "Đồng Việt Nam"},
                new DropdownListItem{Key = "2", Value = "USD" , Desc = "Dola Mỹ"},
                new DropdownListItem{Key = "3", Value = "SGD" , Desc = "Dola Singapore"},
            };
        }

        public List<DropdownListItem> GeContractTypes()
        {
            return new List<DropdownListItem>() {
                new DropdownListItem{Key = "1", Value = "VND" , Desc = "Đồng Việt Nam"},
                new DropdownListItem{Key = "2", Value = "USD" , Desc = "Dola Mỹ"},
                new DropdownListItem{Key = "3", Value = "SGD" , Desc = "Dola Singapore"},
            };
        }

        public List<DropdownListItem> GetTaxes()
        {
            return new List<DropdownListItem>() {
                new DropdownListItem{Key = "0", Value = "0%"},
                new DropdownListItem{Key = "5", Value = "5%"},
                new DropdownListItem{Key = "10", Value = "10%"}
            };
        }


        #endregion ===========================================================================================================

        #region File ============================================================================================================

        public List<DropdownListItem> GetFileTypes()
        {
            List<DropdownListItem> lstFile = new List<DropdownListItem>
            {
                new DropdownListItem {Key = "ai", Value = "/Images/ImageType/ai.png"},
                new DropdownListItem {Key = "bmp", Value = "/Images/ImageType/bmp.png"},
                new DropdownListItem {Key = "doc", Value = "/Images/ImageType/doc.png"},
                new DropdownListItem {Key = "docx", Value = "/Images/ImageType/doc.png"},
                new DropdownListItem {Key = "xls", Value = "/Images/ImageType/xls.png"},
                new DropdownListItem {Key = "xlsx", Value = "/Images/ImageType/xls.png"},
                new DropdownListItem {Key = "ppt", Value = "/Images/ImageType/ppt.png"},
                new DropdownListItem {Key = "pptx", Value = "/Images/ImageType/ppt.png"},
                new DropdownListItem {Key = "eps", Value = "/Images/ImageType/eps.png"},
                new DropdownListItem {Key = "exe", Value = "/Images/ImageType/exe.png"},
                new DropdownListItem {Key = "gif", Value = "/Images/ImageType/gif.png"},
                new DropdownListItem {Key = "jpg", Value = "/Images/ImageType/jpg.png"},
                new DropdownListItem {Key = "jpeg", Value = "/Images/ImageType/jpg.png"},
                new DropdownListItem {Key = "pdf", Value = "/Images/ImageType/pdf.png"},
                new DropdownListItem {Key = "png", Value = "/Images/ImageType/png.png"},
                new DropdownListItem {Key = "psd", Value = "/Images/ImageType/psd.png"},
                new DropdownListItem {Key = "tiff", Value = "/Images/ImageType/tiff.png"},
                new DropdownListItem {Key = "Data", Value = "/Images/ImageType/Data.png"},
                new DropdownListItem {Key = "mp3", Value = "/Images/ImageType/mp3.png"},
                new DropdownListItem {Key = "rar", Value = "/Images/ImageType/rar.png"},
                new DropdownListItem {Key = "zip", Value = "/Images/ImageType/rar.png"},
                new DropdownListItem {Key = "mp4", Value = "/Images/ImageType/video.png"},
                new DropdownListItem {Key = "avi", Value = "/Images/ImageType/video.png"},
                new DropdownListItem {Key = "flv", Value = "/Images/ImageType/video.png"}
            };
            return lstFile;
        }

        #endregion ===========================================================================================================
        
        //public List<SYS_spfrmApproveOnlineProcess_Result> GetApprovePermission(int? CurrentEmpId)
        //{
        //    HttpContext httpConText = HttpContext.Current;
        //    if (httpContext.ApplicationInstance.Session["ApprovePermission"] == null)
        //    {
        //        List<SYS_spfrmApproveOnlineProcess_Result> list = context.SYS_spfrmApproveOnlineProcess(CurrentEmpId).ToList();
        //        httpContext.ApplicationInstance.Session["ApprovePermission"] = list;
        //        return list;
        //    }
        //    else
        //    {
        //        List<SYS_spfrmApproveOnlineProcess_Result> list = (List<SYS_spfrmApproveOnlineProcess_Result>)httpContext.ApplicationInstance.Session["ApprovePermission"];
        //        return list;
        //    }
        //}
        // statusPlan, currentEmpId, empIDPlan => dùng để set quyền 3 nút reset, save và send
        // statusPlan, FunctionID, CurrentLevelApprove,maxLevelApprove, planId => dùng để set quyền 2 nút approve và unapprove
        //public void CheckPermission(int statusPlan, int? currentEmpId, int empIdPlan, int functionId, int currentLevelApprove, int maxLevelApprove, int planID, ref bool DisabledSaveAndSend, ref bool DisabledApproveAndUnapprove, int StatusProcess = 1)
        //{
        //    if (StatusProcess == 1)
        //    {
        //        #region//Dùng để phân quyền 3 nút "Reset", "Save" và "Send for approval"
        //        // nếu không phải người tạo xem thì ẩn luôn
        //        if (currentEmpId != empIdPlan)
        //        {
        //            DisabledSaveAndSend = false;
        //        }
        //        //Kiểm tra 3 nút đầu
        //        //Nếu là bị cấp 1 không duyệt cho phép chỉnh sửa
        //        else if ((statusPlan == 3 || statusPlan == 5 || statusPlan == 7 || statusPlan == 9 || statusPlan == 11) && currentLevelApprove == 0)
        //        {
        //            DisabledSaveAndSend = true;
        //        }
        //        //Nếu đã gửi đi thì không được quyền chỉnh sửa
        //        else if (planID != 0 && (statusPlan != 0 || (empIdPlan != currentEmpId)))
        //        {
        //            DisabledSaveAndSend = false;
        //        }
        //        //Nếu chưa gửi được quyền chỉnh sửa
        //        else
        //        {
        //            DisabledSaveAndSend = true;
        //        }
        //        #endregion
        //        #region//Kiểm tra 2 nút "Approve" và "Unapprove"

        //        // Bước 1 kiểm tra plan này khi đang là ở max level approve và đã được duyệt thì ẩn. không thì set tiếp
        //        if (currentLevelApprove == maxLevelApprove && ((maxLevelApprove * 2) == statusPlan || (maxLevelApprove * 2 + 1) == statusPlan))
        //        {
        //            DisabledApproveAndUnapprove = false;
        //        }
        //        else
        //        {
        //            //CommonRepository _commonRepository = new CommonRepository(context);
        //            //var ListPermission = _commonRepository.GetApprovePermission(currentEmpId).ToList();
        //            ////Chức năng 456 = "Recruitment Plan"


        //            ////Các phòng được duyệt (Level2ID)
        //            //List<int> SectionPermisstion = new List<int>();
        //            //if (ListPermission != null && ListPermission.Count > 0)
        //            //{
        //            //    SectionPermisstion = ListPermission.Where(p => p.FunctionId == FunctionID).Select(p => p.LSCompanyID).ToList();
        //            //}
        //            ////Nếu kế hoạch này được User thuộc phòng ban nằm trong list trên (1) 
        //            //// VÀ được quyền approve trùng với tình trạng approve của kế hoạch này (2)

        //            //var empModel = context.HR_tblEmp.Where(p => p.EmpId == empIDPlan).FirstOrDefault();
        //            ////1
        //            //if (empModel != null && SectionPermisstion.Contains(empModel.LSCompanyID))
        //            //{
        //            // nếu mà được quyền approve phòng này thì set tiếp
        //            //2
        //            //Set xem có đúng là đang ở cấp approve
        //            // user này có quyền approve ở các cấp nằm trong permisson
        //            //    var permisson = ListPermission.Where(p => p.FunctionId == FunctionID && p.LSCompanyId == empModel.LSCompanyID).FirstOrDefault();
        //            //    bool checkResult = false;
        //            //    switch (CurrentLevelApprove)
        //            //    {
        //            //        case 1:
        //            //            if (permisson.ApproveLevel1 == true)
        //            //            {
        //            //                checkResult = true;
        //            //            }
        //            //            break;
        //            //        case 2:
        //            //            if (permisson.ApproveLevel2 == true)
        //            //            {
        //            //                checkResult = true;
        //            //            }
        //            //            break;
        //            //        case 3:
        //            //            if (permisson.ApproveLevel3 == true)
        //            //            {
        //            //                checkResult = true;
        //            //            }
        //            //            break;
        //            //        case 4:
        //            //            if (permisson.ApproveLevel4 == true)
        //            //            {
        //            //                checkResult = true;
        //            //            }
        //            //            break;
        //            //        case 5:
        //            //            if (permisson.ApproveLevel5 == true)
        //            //            {
        //            //                checkResult = true;
        //            //            }
        //            //            break;
        //            //    }
        //            //    DisabledApproveAndUnapprove = checkResult;
        //            //}
        //            //else
        //            //{
        //            //    DisabledApproveAndUnapprove = false;
        //            //}
        //        }
        //        #endregion

        //        #region // Cập nhật nếu là cấp trên thì được "duyệt cấp 1"
        //        //if (CurrentLevelApprove == 1)
        //        //{
        //        //    DisabledSaveAndSend = false;
        //        // var Id = context.HR_tblEmp.Where(p => p.EmpId == empIDPlan).Select(p => p.LineManagerID).FirstOrDefault();
        //        //if (currentEmpId == id)
        //        //{
        //        //DisabledApproveAndUnapprove = true;
        //        //}
        //        //else
        //        //{
        //        //    DisabledApproveAndUnapprove = false;
        //        //}
        //        //}
        //        #endregion
        //    }
        //    else
        //    {
        //        DisabledSaveAndSend = false;
        //        DisabledApproveAndUnapprove = false;
        //    }
        //}
    }
}

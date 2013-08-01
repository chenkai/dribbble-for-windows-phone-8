using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoCommon.EntityModel
{
    public class CustomerLiveTitleData
    {
        public string Temperature { get; set; }
        public string WeatherInfo { get; set; }
        public string HighestTemp { get; set; }
        public string LowestTemp { get; set; }

        public string MonthAndDay
        {
            get
            {
                return ConvertToMonthNum(DateTime.Now.Month)+"月"+ConvertToDayNum(DateTime.Now.Day)+"日";
            }            
        }
        public string WeekOfDay
        {
            get
            {
                return  DateTime.Now.DayOfWeek.ToString();
            } 
        }

        public string CityId { get; set; }
        public string CityName { get; set; }

        public string WideBackgroundImageUrl { get; set; }
        public string MiddleBackgrondImageUrl { get; set; }
        public string SmallBackgroundImageUrl { get; set; }

        private string ConvertToMonthNum(int monthNum)
        {
            string mouthNumber = string.Empty;

            List<KeyValuePair<string, int>> mouthNumList = new List<KeyValuePair<string, int>>();
            mouthNumList.Add(new KeyValuePair<string, int>("一", 1));
            mouthNumList.Add(new KeyValuePair<string, int>("二", 2));
            mouthNumList.Add(new KeyValuePair<string, int>("三", 3));

            mouthNumList.Add(new KeyValuePair<string, int>("四", 4));
            mouthNumList.Add(new KeyValuePair<string, int>("五", 5));
            mouthNumList.Add(new KeyValuePair<string, int>("六", 6));

            mouthNumList.Add(new KeyValuePair<string, int>("七", 7));
            mouthNumList.Add(new KeyValuePair<string, int>("八", 8));
            mouthNumList.Add(new KeyValuePair<string, int>("九", 9));

            mouthNumList.Add(new KeyValuePair<string, int>("十", 10));
            mouthNumList.Add(new KeyValuePair<string, int>("十一", 11));
            mouthNumList.Add(new KeyValuePair<string, int>("十二", 12));

            foreach (KeyValuePair<string, int> queryArgu in mouthNumList)
            {
                if (queryArgu.Value.Equals(monthNum))
                {
                    mouthNumber = queryArgu.Key;
                    break;
                }
            }
            return mouthNumber;
        }
        private string ConvertToDayNum(int day)
        {
            string dayStr = string.Empty;
            List<KeyValuePair<int, string>> dayNumberList = new List<KeyValuePair<int, string>>();
            dayNumberList.Add(new KeyValuePair<int, string>(1, "一"));
            dayNumberList.Add(new KeyValuePair<int, string>(2, "二"));
            dayNumberList.Add(new KeyValuePair<int, string>(3, "三"));

            dayNumberList.Add(new KeyValuePair<int, string>(4, "四"));
            dayNumberList.Add(new KeyValuePair<int, string>(5, "五"));
            dayNumberList.Add(new KeyValuePair<int, string>(6, "六"));

            dayNumberList.Add(new KeyValuePair<int, string>(7, "七"));
            dayNumberList.Add(new KeyValuePair<int, string>(8, "八"));
            dayNumberList.Add(new KeyValuePair<int, string>(9, "九"));

            dayNumberList.Add(new KeyValuePair<int, string>(10, "十"));
            dayNumberList.Add(new KeyValuePair<int, string>(11, "十一"));
            dayNumberList.Add(new KeyValuePair<int, string>(12, "十二"));

            dayNumberList.Add(new KeyValuePair<int, string>(13, "十三"));
            dayNumberList.Add(new KeyValuePair<int, string>(14, "十四"));
            dayNumberList.Add(new KeyValuePair<int, string>(15, "十五"));

            dayNumberList.Add(new KeyValuePair<int, string>(16, "十六"));
            dayNumberList.Add(new KeyValuePair<int, string>(17, "十七"));
            dayNumberList.Add(new KeyValuePair<int, string>(18, "十八"));


            dayNumberList.Add(new KeyValuePair<int, string>(19, "十九"));
            dayNumberList.Add(new KeyValuePair<int, string>(20, "二十"));
            dayNumberList.Add(new KeyValuePair<int, string>(21, "二十一"));

            dayNumberList.Add(new KeyValuePair<int, string>(22, "二十二"));
            dayNumberList.Add(new KeyValuePair<int, string>(23, "二十三"));
            dayNumberList.Add(new KeyValuePair<int, string>(24, "二十四"));

            dayNumberList.Add(new KeyValuePair<int, string>(25, "二十五"));
            dayNumberList.Add(new KeyValuePair<int, string>(26, "二十六"));
            dayNumberList.Add(new KeyValuePair<int, string>(27, "二十七"));

            dayNumberList.Add(new KeyValuePair<int, string>(28, "二十八"));
            dayNumberList.Add(new KeyValuePair<int, string>(29, "二十九"));
            dayNumberList.Add(new KeyValuePair<int, string>(30, "三十"));

            dayNumberList.Add(new KeyValuePair<int, string>(31, "三十一"));

            foreach (KeyValuePair<int, string> queryArgu in dayNumberList)
            {
                if (queryArgu.Key.Equals(day))
                {
                    dayStr = queryArgu.Value;
                    break;
                }
            }
            return dayStr;
        }

        public string Air { get; set; }
        public string Dress { get; set; }
        public string Wind { get; set; }

        public bool IsHaveWarn { get; set; }
        public string WarnId { get; set; }
    }
}

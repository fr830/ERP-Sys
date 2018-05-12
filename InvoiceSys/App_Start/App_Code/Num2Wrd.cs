using System;
using System.Collections.Generic;
using System.Text;

    public class NumberToEnglish
    {

        public String changeNumericToWords(double numb)
        {
            String num = numb.ToString();
            return changeToWords(num, false);
        }

        public String changeCurrencyToWords(String numb)
        {
            return changeToWords(numb, true);
        }

        public String changeNumericToWords(String numb)
        {
            return changeToWords(numb, false);
        }

        public String changeCurrencyToWords(double numb)
        {
            return changeToWords(numb.ToString(), true);
        }

        private String changeToWords(String numb, bool isCurrency)
        {
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            String endStr = (isCurrency) ? ("Only") : ("");
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = (isCurrency) ? ("and") : ("point");// just to separate whole numbers from points/Rupees
                        endStr = (isCurrency) ? ("Cents " + endStr) : ("");
                        pointStr = translateRupees(points);
                    }
                }
                val = String.Format("{0} {1}{2} {3}", translateWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch
            {
                ;
            }
            return val;
        }

        private String translateWholeNumber(String number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX
                bool isDone = false;//test if already translated
                double dblAmt = (Convert.ToDouble(number));
                //if ((dblAmt > 0) && number.StartsWith("0"))

                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric
                    beginsZero = number.StartsWith("0");
                    int numDigits = number.Length;
                    int pos = 0;//store digit grouping
                    String place = "";//digit grouping name:hundres,thousand,etc...
                    switch (numDigits)
                    {
                        case 1://ones' range
                            word = ones(number);
                            isDone = true;
                            break;
                        case 2://tens' range
                            word = tens(number);
                            isDone = true;
                            break;
                        case 3://hundreds' range
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4://thousands' range
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 7://millions' range
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;
                        case 10://Billions's range
                            pos = (numDigits % 10) + 1;
                            place = " Billion ";
                            break;
                        //add extra case options for anything above Billion...
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)
                        word = translateWholeNumber(number.Substring(0, pos)) + place + translateWholeNumber(number.Substring(pos));
                        //check for trailing zeros
                        if (beginsZero) word = " and " + word.Trim();
                    }
                    //ignore digit grouping names
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
                catch
                {
                    ;
                }
            return word.Trim();
        }

        private String tens(String digit)
        {
            int digt = Convert.ToInt32(digit);
            String name = null;
            switch (digt)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (digt > 0)
                    {
                        name = tens(digit.Substring(0, 1) + "0") + " " + ones(digit.Substring(1));
                    }
                    break;
            }
            return name;
        }

        private String ones(String digit)
        {
            int digt = Convert.ToInt32(digit);
            String name = "";
            switch (digt)
            {
                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }

        private String ones1(String digit)
        {
            int digt = Convert.ToInt32(digit);
            String name = "";
            switch (digt)
            {
                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }

        private String translateRupees(String Rupees)
        {
            String cts = "", digit = "", engOne = "";
            for (int i = 0; i < Rupees.Length; i++)
            {
                digit = Rupees[i].ToString();
                if (digit.Equals("0"))
                {
                        engOne = "Zero";
                }
                else
                {
                        engOne = ones(digit);
                }
                cts += " " + engOne;
            }
            return cts;
        }

        public String NumberToWord(int num)
        {
            if (num == 0)
                return "Zero";
 
            if (num < 0)
                return "Not supported";
 
            var words = "";
            string[] strones = { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            string[] strtens = { "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
 

            int crore = 0, lakhs = 0, thousands = 0, hundreds = 0, tens = 0, single = 0;
 

            crore = num / 10000000; num = num - crore * 10000000;
            lakhs = num / 100000;   num = num - lakhs * 100000;
            thousands = num / 1000; num = num - thousands * 1000;
            hundreds = num / 100;   num = num - hundreds * 100;
            if (num > 19)
            {
                tens = num / 10;    num = num - tens * 10;
            }
            single = num;
 

            if (crore > 0)
            {
                if (crore > 19)
                    words += NumberToWord(crore) + "Crore ";
                else
                    words += strones[crore - 1] + " Crore ";
            }
 
            if (lakhs > 0)
            {
                if (lakhs > 19)
                    words += NumberToWord(lakhs) + "Lakh ";
                else
                    words += strones[lakhs - 1] + " Lakh ";
            }
 
            if (thousands > 0)
            {
                if (thousands > 19)
                    words += NumberToWord(thousands) + "Thousand ";
                else
                    words += strones[thousands - 1] + " Thousand ";
            }
 
            if (hundreds > 0)
                words += strones[hundreds - 1] + " Hundred ";
 
            if (tens > 0)
                words += strtens[tens - 2] + " ";
 
            if (single > 0)
                words += strones[single - 1] + " ";
 
            return words;
        }

        public String NumberToCurrencyText(decimal number)
        {
            // Round the value just in case the decimal value is longer than two digits
            number = decimal.Round(number, 2);

            string wordNumber = string.Empty;

            // Divide the number into the whole and fractional part strings
            string[] arrNumber = number.ToString().Split('.');

            // Get the whole number text
            long wholePart = long.Parse(arrNumber[0]);
            string strWholePart = NumberToText(wholePart);

            // For amounts of zero dollars show 'No Dollars...' instead of 'Zero Dollars...'
                wordNumber = (wholePart == 0 ? " " : strWholePart) + (wholePart == 1 ? " and " : " and ");

            // If the array has more than one element then there is a fractional part otherwise there isn't
            // just add 'No Cents' to the end
            if (arrNumber.Length > 1)
            {
                // If the length of the fractional element is only 1, add a 0 so that the text returned isn't,
                // 'One', 'Two', etc but 'Ten', 'Twenty', etc.
                long fractionPart = long.Parse((arrNumber[1].Length == 1 ? arrNumber[1] + "0" : arrNumber[1]));
                string strFarctionPart = NumberToText(fractionPart);

                if (fractionPart == 0){
                    wordNumber = ReplaceLastOccurrence(wordNumber, "and", "Only");
                }
                else{
                    wordNumber += (fractionPart == 0 ? " " : strFarctionPart) + (fractionPart == 1 ? " Cent Only" : " Cents Only");
                }
            }
            else
                wordNumber = ReplaceLastOccurrence(wordNumber, "and", "Only");

            return wordNumber;
        }

        public String NumberToText(long number)
        {
        StringBuilder wordNumber = new StringBuilder();                       
 
        string[] powers = new string[] { "Thousand ", "Million ", "Billion " };
        string[] tens = new string[] { "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
        string[] ones = new string[] { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", 
                                       "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
 
        if (number == 0) { return "Zero"; }
        if (number < 0) 
        { 
            wordNumber.Append("Negative ");
            number = -number;
        }
 
        long[] groupedNumber = new long[] { 0, 0, 0, 0 };
        int groupIndex = 0;
 
        while (number > 0)
        {
            groupedNumber[groupIndex++] = number % 1000;
            number /= 1000;
        }
 
        for (int i = 3; i >= 0; i--)
        {
            long group = groupedNumber[i];
 
            if (group >= 100)
            {
                wordNumber.Append(ones[group / 100 - 1] + " Hundred ");
                group %= 100;
 
                if (group == 0 && i > 0)
                    wordNumber.Append(powers[i - 1]);
            }
 
            if (group >= 20)
            {
                if ((group % 10) != 0)
                    wordNumber.Append(tens[group / 10 - 2] + " " + ones[group % 10 - 1] + " ");
                else
                    wordNumber.Append(tens[group / 10 - 2] + " ");                    
            }
            else if (group > 0)
                wordNumber.Append(ones[group - 1] + " ");
 
            if (group != 0 && i > 0)
                wordNumber.Append(powers[i - 1]);
        }
 
        return wordNumber.ToString().Trim();
    }
        
        private string ReplaceLastOccurrence(string Source, string Find, string Replace)
        {
            int place = Source.LastIndexOf(Find);

            if (place == -1)
                return Source;

            string result = Source.Remove(place, Find.Length).Insert(place, Replace);
            return result;
        }
    }
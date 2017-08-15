//Zamiana liczby na slowa z polska gramatyka
//Lesław Pawlaczyk
//www.algorytm.org

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LanguageModelFilter
{
	public class NumbersToText
	{
		# region Constant Arrays

		protected static string[] SINGLES       = { "", "jeden", "dwa", "trzy", "cztery", "pięć", "sześć", "siedem", "osiem", "dziewięć" };
		protected static string[] TEENS         = { "", "jedenaście", "dwanaście", "trzynaście", "czternaście", "piętnaście", "szesnaście", "siedemnaście", "osiemnaście", "dziewiętnaście" };
		protected static string[] TENS          = { "", "dziesięć", "dwadzieścia", "trzydzieści", "czterdzieści", "pięćdziesiąt", "sześćdziesiąt", "siedemdziesiąt", "osiemdziesiąt", "dziewięćdziesiąt" };
		protected static string[] HUNDREDS      = { "", "sto", "dwieście", "trzysta", "czterysta", "pięćset", "sześćset", "siedemset", "osiemset", "dziewięćset" };
		protected static string[,] CONJUGATIONS = new string[,]
			{
				{ "", "", ""                           },
				{ "tysiąc", "tysiące", "tysięcy"       },
				{ "milion" , "miliony" , "milionów"    },
				{ "miliard", "miliardy", "miliardów"   },
				{ "bilion" , "biliony" , "bilionow"    },	
				{ "biliard", "biliardy", "biliardow"   },
				{ "trylion" , "tryliony", "tryliardow" }
			};

		# endregion // Constant Arrays

		# region Implementation

		public static String DigitsStringToSpokenString(string input)
		{
			StringBuilder builder = new StringBuilder();

			Int64 number = 0;
			if (!Int64.TryParse(input, out number))
			{
				return input;
			}

			if (number < 0)
			{
				builder.Append("minus ");
				number = -number;
			}

			Int64 sHundreds    = 0;
			Int64 dTens        = 0;
			Int64 jSingles     = 0;
			Int64 nTeens       = 0;
			Int64 gMagnitude   = 0;
			Int64 kConjugation = 0;
			
			List<string> groups = new List<string>();

			while (number != 0)
			{
 				//get hundreds, tens and singles from a number to process
				sHundreds = (number % 1000) / 100;
				dTens     = (number % 100) / 10;
				jSingles  = number % 10;

				//check if teens form should be used
				if (dTens == 1 && jSingles > 0)
				{
					nTeens   = jSingles;
					dTens    = 0;
					jSingles = 0;
				}
				else
				{
					nTeens = 0;
				}
				
				//choose conjugation form
				if (jSingles == 1 && sHundreds + dTens + nTeens == 0)
				{
					kConjugation = 0;
				}
				else if (jSingles >= 2 && jSingles <= 4)
				{
					kConjugation = 1;
				}
				else
				{
					kConjugation = 2;
				}

				//add text if there is any houndred, ten, teen or single
				if (sHundreds + dTens + nTeens + jSingles > 0)
				{
					if (jSingles == 1 && !String.IsNullOrWhiteSpace(CONJUGATIONS[gMagnitude, kConjugation]))
					{
						// we do not say 'jeden tysiąc' but 'tysiąc'
						jSingles = 0;
					}
					groups.Add(string.Format(" {0} {1} {2} {3} {4}", HUNDREDS[sHundreds], TENS[dTens], TEENS[nTeens], SINGLES[jSingles], CONJUGATIONS[gMagnitude, kConjugation]));
				}

				//process next three digits
				gMagnitude++;
				number = number / 1000;
			}

			groups.Reverse();
			groups.ForEach(x => builder.Append(x));

			string result = builder.ToString();
			result = Regex.Replace(result, @"\s+", " ").Trim();

			return result;
		}		

		# endregion // Implementation


        public static string ConvertAmountInPLN(string input)
        {
            string[] amount = input.Split(',');
            return string.Format("{0} zł {1}/100", DigitsStringToSpokenString(amount[0]), amount[1]);
        }
    }
}

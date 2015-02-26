using System;
using System.Collections.Generic;
using System.Linq;

namespace dndgen_console
{
	public class DnDRandom
	{
		public enum DNDDice : int
		{
			DND_D4=1,
			DND_D6,
			DND_D8,
			DND_D10,
			DND_D12,
			DND_D20
		};

		static readonly char[] validOps = {'d', '+', '-', '*', '/'};
		static readonly char[][] opPriority = { new char[] { 'd' }, new char[] { '*', '/' }, new char[] { '+', '-' } };

		readonly Random rand;

		public DnDRandom()
		{
			rand = new Random();
		}

		public DnDRandom(int seed)
		{
			rand = new Random(seed);
		}

		public int getDieType(int numSides)
		{
			if(numSides < 4 || numSides > 20 || numSides % 2 == 1)
				return 0;

			switch(numSides)
			{
				case 4:
					return (int)DNDDice.DND_D4;
				case 6:
					return (int)DNDDice.DND_D6;
				case 8:
					return (int)DNDDice.DND_D8;
				case 10:
					return (int)DNDDice.DND_D10;
				case 12:
					return (int)DNDDice.DND_D12;
				case 20:
					return (int)DNDDice.DND_D20;
				default:
					return 0;
			}
		}

		public int rollValue(int diceType, int numDice=1, int mod=0)
		{
			if(numDice <= 0)
				numDice = 1;

			int maxRoll;

			switch(diceType)
			{

				case (int)DNDDice.DND_D4:
					maxRoll = 4;
					break;
				case (int)DNDDice.DND_D6:
					maxRoll = 6;
					break;
				case (int)DNDDice.DND_D8:
					maxRoll = 8;
					break;
				case (int)DNDDice.DND_D10:
					maxRoll = 10;
					break;
				case (int)DNDDice.DND_D12:
					maxRoll = 12;
					break;
				case (int)DNDDice.DND_D20:
					maxRoll = 21;
					break;
				default:
					return 0;
			}

			int val = 0;
			for(int i = 0; i < numDice; ++i)
				val += rand.Next(1, maxRoll);

			return val + mod;
		}

		public int[] rollArray(int diceType, int numDice=1)
		{
			if(numDice <= 0)
				numDice = 1;

			int maxRoll;

			switch(diceType)
			{
				case (int)DNDDice.DND_D4:
					maxRoll = 5;
					break;
				case (int)DNDDice.DND_D6:
					maxRoll = 7;
					break;
				case (int)DNDDice.DND_D8:
					maxRoll = 9;
					break;
				case (int)DNDDice.DND_D10:
					maxRoll = 11;
					break;
				case (int)DNDDice.DND_D12:
					maxRoll = 13;
					break;
				case (int)DNDDice.DND_D20:
					maxRoll = 21;
					break;
				default:
					return new int[]{0};
			}

			int[] val;
			val = new int[numDice];
			for(int i = 0; i < numDice; ++i)
				val[i] = rand.Next(1, maxRoll);

			return val;
		}

		/*public List<int> parseRoll(string roll)
		{
			roll.Trim();
			roll.Replace(" ", string.Empty);
			roll.Replace("\t", string.Empty);

			char[] chroll = roll.ToCharArray();
			List<int> nums = new List<int>();
			List<char> ops = new List<char>();
			List<int> rolls = new List<int>();

			string stri;
			char chi;
			string currNum;
			int outpn;
			char last;

			for(int i; i < chroll.Length; i++)
			{
				chi = chroll[i];
				stri = chi.ToString();

				if(int.TryParse(stri))
				{
					currNum += stri;
				}
				else
				{
					if(currNum.Length > 0)
					{
						if(int.TryParse(currNum, out outpn))
						{
							nums.Add(outpn);
						}
						currNum = string.Empty;
					}

					if("d+-/*".Contains(stri))
					{
						if(last == 'd' && chi == 'd')
							return null;

						ops.Add(chi);
						last = chi;
					}
					else
						return null;
				}
			}

			int idx;
			char curr;
			while((idx = ops.Count))
			{
				while(0 <= --idx)
				{
					curr = ops[idx];
					if(idx < ops.Count - 1)
					{
						last = ops[idx + 1];

						if(opPrec[last] > opPrec[curr])
						{

						}
					}

				}
			}
		}*/

		public int parseSimpleOperation(char op, int pre, int post, ref List<int> rolls)
		{
			switch(op)
			{
				case 'd':
					var arr = rollArray(getDieType(post), pre);

					rolls.AddRange(arr);

					return arr.Sum();
				case '+':
					return pre + post;
				case '-':
					return pre - post;
				case '*':
					return pre * post;
				case '/':
					return (int)Math.Ceiling((double)pre / (double)post);
				default:
					return 0;
			}
		}

		public int parseSimpleOperation(char op, int pre, int post)
		{
			switch(op)
			{
				case 'd':
					return rollValue(getDieType(post), pre);
				case '+':
					return pre + post;
				case '-':
					return pre - post;
				case '*':
					return pre * post;
				case '/':
					return (int)Math.Ceiling((double)pre / (double)post);
				default:
					return 0;
			}
		}

		public int parseRoll(string roll)
		{
			//int total;

			roll = roll.Replace(" ", string.Empty);
			roll = roll.Replace("\t", string.Empty);

			var nums = new List<int>();
			var ops = new List<char>();

			int tmp;
			string currNum = null;
			char c;
			string sc;
			for(int i = 0; i < roll.Length; i++)
			{
				c = roll[i];
				sc = c.ToString();

				if(int.TryParse(sc, out tmp))
				{
					currNum += sc;
				}
				else
				{
					if(currNum != null)
					{
						nums.Add(int.Parse(currNum));
						currNum = null;
					}
					else if(c == 'd')
						nums.Add(1);

					if(validOps.Contains(c))
						ops.Add(c);
				}
			}

			if(currNum != null)
				nums.Add(int.Parse(currNum));

			while(nums.Count > 1)
			{
				foreach(char[] oplist in opPriority)
				{
					foreach(char op in oplist)
					{
						for(int i = 0; i < ops.Count; i++)
						{
							if(op != ops[i])
								continue;

							if(i >= nums.Count - 1)
								break;

							var preNum = nums[i];
							var postNum = nums[i + 1];

							var result = parseSimpleOperation(op, preNum, postNum);

							nums.RemoveRange(i, 2);
							nums.Insert(i, result);
							ops.RemoveAt(i);
							i--;
						}
					}
				}

				if(ops.Count == 0)
					break;
			}

			return nums[0];
		}

		public string[] parseRollExpanded(string roll)
		{
			//int total;

			roll = roll.Replace(" ", string.Empty);
			roll = roll.Replace("\t", string.Empty);

			var nums = new List<int>();
			var ops = new List<char>();
			var rolls = new List<int>();

			int tmp;
			string currNum = null;
			char c;
			string sc;
			for(int i = 0; i < roll.Length; i++)
			{
				c = roll[i];
				sc = c.ToString();

				if(int.TryParse(sc, out tmp))
				{
					currNum += sc;
				}
				else
				{
					if(currNum != null)
					{
						nums.Add(int.Parse(currNum));
						currNum = null;
					}
					else if(c == 'd')
						nums.Add(1);

					if(validOps.Contains(c))
						ops.Add(c);
				}
			}

			if(currNum != null)
				nums.Add(int.Parse(currNum));

			while(nums.Count > 1)
			{
				foreach(char[] oplist in opPriority)
				{
					foreach(char op in oplist)
					{
						for(int i = 0; i < ops.Count; i++)
						{
							if(op != ops[i])
								continue;

							if(i >= nums.Count - 1)
								break;

							var preNum = nums[i];
							var postNum = nums[i + 1];

							if(op == 'd')
								rolls.Clear();

							var result = parseSimpleOperation(op, preNum, postNum, ref rolls);

							nums.RemoveRange(i, 2);
							nums.Insert(i, result);
							ops.RemoveAt(i);
							i--;
						}
					}
				}

				if(ops.Count == 0)
					break;
			}

			string[] output = { string.Join(", ", nums.ToArray()), string.Join(", ", rolls.ToArray()) };
			return output;
		}
			
	}
}


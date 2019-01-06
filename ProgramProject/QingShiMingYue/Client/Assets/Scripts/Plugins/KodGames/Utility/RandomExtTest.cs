using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KodGames.Utility
{
	public class ExtRandomTest : MonoBehaviour
	{
		private void Awake()
		{
			SplitChanceTest();
			ChanceTest();
			ChoiceTestArray();
			ChoiceTestList();
			WeightedChoiceTestArray();
			WeightedChoiceTestList();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.O))
			{
				ShuffleTestArray();
			}
			else if (Input.GetKeyDown(KeyCode.P))
			{
				ShuffleTestList();
			}
		}

		private void SplitChanceTest()
		{
			int nSplitChanceTest1 = 0;
			int nSplitChanceTest2 = 0;
			for (int i = 0; i < 50000000; i++)
			{
				if (ExtRandom<int>.SplitChance())
				{
					nSplitChanceTest1++;
				}
				else
				{
					nSplitChanceTest2++;
				}
			}
			Debug.Log("After 50mil tests, SplitChance() results are as follows:");
			Debug.Log("First half: " + nSplitChanceTest1.ToString());
			Debug.Log("Second half: " + nSplitChanceTest2.ToString());
		}

		private void ChanceTest()
		{
			int nProbabilityFactor = 32;
			int nProbabilitySpace = 100;
			float fChanceExpected1 = (float)nProbabilityFactor / (float)nProbabilitySpace;
			float fChanceExpected2 = ((float)nProbabilitySpace - (float)nProbabilityFactor) / (float)nProbabilitySpace;
			int nChanceTest1 = 0;
			int nChanceTest2 = 0;
			for (int i = 0; i < 50000000; i++)
			{
				if (ExtRandom<int>.Chance(nProbabilityFactor, nProbabilitySpace))
				{
					nChanceTest1++;
				}
				else
				{
					nChanceTest2++;
				}
			}
			float fChanceTest1 = (float)nChanceTest1 / 50000000f;
			float fChanceTest2 = (float)nChanceTest2 / 50000000f;
			Debug.Log("After 50mil tests, Chance() results are as follows:");
			Debug.Log("First half: Expected: " + fChanceExpected1.ToString() + "  ; Actual: " + fChanceTest1.ToString());
			Debug.Log("Second half: Expected: " + fChanceExpected2.ToString() + "  ; Actual: " + fChanceTest2.ToString());
		}

		private void ChoiceTestArray()
		{
			string[] strChoices = new string[6] { "who", "what", "where", "when", "why", "how" };
			int[] nChoiceTest = new int[6];
			for (int i = 0; i < nChoiceTest.Length; i++)
			{
				nChoiceTest[i] = 0;
			}
			for (int i = 0; i < 50000000; i++)
			{
				switch (ExtRandom<string>.Choice(strChoices))
				{
					case "who":
						{
							nChoiceTest[0]++;
							break;
						}
					case "what":
						{
							nChoiceTest[1]++;
							break;
						}
					case "where":
						{
							nChoiceTest[2]++;
							break;
						}
					case "when":
						{
							nChoiceTest[3]++;
							break;
						}
					case "why":
						{
							nChoiceTest[4]++;
							break;
						}
					case "how":
						{
							nChoiceTest[5]++;
							break;
						}
				}
			}
			Debug.Log("After 50mil tests, Choice() results are as follows:");
			Debug.Log("First: " + nChoiceTest[0].ToString());
			Debug.Log("Second: " + nChoiceTest[1].ToString());
			Debug.Log("Third: " + nChoiceTest[2].ToString());
			Debug.Log("Fourth: " + nChoiceTest[3].ToString());
			Debug.Log("Fifth: " + nChoiceTest[4].ToString());
			Debug.Log("Sixth: " + nChoiceTest[5].ToString());
		}

		private void ChoiceTestList()
		{
			List<string> strChoices = new List<string>(6) { "who", "what", "where", "when", "why", "how" };
			int[] nChoiceTest = new int[6];
			for (int i = 0; i < nChoiceTest.Length; i++)
			{
				nChoiceTest[i] = 0;
			}
			for (int i = 0; i < 50000000; i++)
			{
				switch (ExtRandom<string>.Choice(strChoices))
				{
					case "who":
						{
							nChoiceTest[0]++;
							break;
						}
					case "what":
						{
							nChoiceTest[1]++;
							break;
						}
					case "where":
						{
							nChoiceTest[2]++;
							break;
						}
					case "when":
						{
							nChoiceTest[3]++;
							break;
						}
					case "why":
						{
							nChoiceTest[4]++;
							break;
						}
					case "how":
						{
							nChoiceTest[5]++;
							break;
						}
				}
			}
			Debug.Log("After 50mil tests, Choice() results are as follows:");
			Debug.Log("First: " + nChoiceTest[0].ToString());
			Debug.Log("Second: " + nChoiceTest[1].ToString());
			Debug.Log("Third: " + nChoiceTest[2].ToString());
			Debug.Log("Fourth: " + nChoiceTest[3].ToString());
			Debug.Log("Fifth: " + nChoiceTest[4].ToString());
			Debug.Log("Sixth: " + nChoiceTest[5].ToString());
		}

		private void WeightedChoiceTestArray()
		{
			string[] strChoices = new string[6] { "who", "what", "where", "when", "why", "how" };
			int[] nWeights = new int[6] { 3, 16, 33, 12, 1, 35 };
			float[] fWeightedChoiceExpected = new float[6];
			int[] nWeightedChoiceTest = new int[6];
			float[] fWeightedChoiceTest = new float[6];
			for (int i = 0; i < nWeights.Length; i++)
			{
				fWeightedChoiceExpected[i] = (float)nWeights[i] / 100f;
				nWeightedChoiceTest[i] = 0;
				fWeightedChoiceTest[i] = 0f;
			}
			for (int i = 0; i < 50000000; i++)
			{
				switch (ExtRandom<string>.WeightedChoice(strChoices, nWeights))
				{
					case "who":
						{
							nWeightedChoiceTest[0]++;
							break;
						}
					case "what":
						{
							nWeightedChoiceTest[1]++;
							break;
						}
					case "where":
						{
							nWeightedChoiceTest[2]++;
							break;
						}
					case "when":
						{
							nWeightedChoiceTest[3]++;
							break;
						}
					case "why":
						{
							nWeightedChoiceTest[4]++;
							break;
						}
					case "how":
						{
							nWeightedChoiceTest[5]++;
							break;
						}
				}
			}
			for (int i = 0; i < nWeights.Length; i++)
			{
				fWeightedChoiceTest[i] = (float)nWeightedChoiceTest[i] / 50000000f;
			}
			Debug.Log("After 50mil tests, WeightedChoice() results are as follows:");
			Debug.Log("First: Expected: " + fWeightedChoiceExpected[0].ToString() + "  ; Actual: " + fWeightedChoiceTest[0].ToString());
			Debug.Log("Second: Expected: " + fWeightedChoiceExpected[1].ToString() + "  ; Actual: " + fWeightedChoiceTest[1].ToString());
			Debug.Log("Third: Expected: " + fWeightedChoiceExpected[2].ToString() + "  ; Actual: " + fWeightedChoiceTest[2].ToString());
			Debug.Log("Fourth: Expected: " + fWeightedChoiceExpected[3].ToString() + "  ; Actual: " + fWeightedChoiceTest[3].ToString());
			Debug.Log("Fifth: Expected: " + fWeightedChoiceExpected[4].ToString() + "  ; Actual: " + fWeightedChoiceTest[4].ToString());
			Debug.Log("Sixth: Expected: " + fWeightedChoiceExpected[5].ToString() + "  ; Actual: " + fWeightedChoiceTest[5].ToString());
		}

		private void WeightedChoiceTestList()
		{
			List<string> strChoices = new List<string>(6) { "who", "what", "where", "when", "why", "how" };
			int[] nWeights = new int[6] { 3, 16, 33, 12, 1, 35 };
			float[] fWeightedChoiceExpected = new float[6];
			int[] nWeightedChoiceTest = new int[6];
			float[] fWeightedChoiceTest = new float[6];
			for (int i = 0; i < nWeights.Length; i++)
			{
				fWeightedChoiceExpected[i] = (float)nWeights[i] / 100f;
				nWeightedChoiceTest[i] = 0;
				fWeightedChoiceTest[i] = 0f;
			}
			for (int i = 0; i < 50000000; i++)
			{
				switch (ExtRandom<string>.WeightedChoice(strChoices, nWeights))
				{
					case "who":
						{
							nWeightedChoiceTest[0]++;
							break;
						}
					case "what":
						{
							nWeightedChoiceTest[1]++;
							break;
						}
					case "where":
						{
							nWeightedChoiceTest[2]++;
							break;
						}
					case "when":
						{
							nWeightedChoiceTest[3]++;
							break;
						}
					case "why":
						{
							nWeightedChoiceTest[4]++;
							break;
						}
					case "how":
						{
							nWeightedChoiceTest[5]++;
							break;
						}
				}
			}
			for (int i = 0; i < nWeights.Length; i++)
			{
				fWeightedChoiceTest[i] = (float)nWeightedChoiceTest[i] / 50000000f;
			}
			Debug.Log("After 50mil tests, WeightedChoice() results are as follows:");
			Debug.Log("First: Expected: " + fWeightedChoiceExpected[0].ToString() + "  ; Actual: " + fWeightedChoiceTest[0].ToString());
			Debug.Log("Second: Expected: " + fWeightedChoiceExpected[1].ToString() + "  ; Actual: " + fWeightedChoiceTest[1].ToString());
			Debug.Log("Third: Expected: " + fWeightedChoiceExpected[2].ToString() + "  ; Actual: " + fWeightedChoiceTest[2].ToString());
			Debug.Log("Fourth: Expected: " + fWeightedChoiceExpected[3].ToString() + "  ; Actual: " + fWeightedChoiceTest[3].ToString());
			Debug.Log("Fifth: Expected: " + fWeightedChoiceExpected[4].ToString() + "  ; Actual: " + fWeightedChoiceTest[4].ToString());
			Debug.Log("Sixth: Expected: " + fWeightedChoiceExpected[5].ToString() + "  ; Actual: " + fWeightedChoiceTest[5].ToString());
		}

		private void ShuffleTestArray()
		{
			string[] strChoices = new string[6] { "who", "what", "where", "when", "why", "how" };
			string[] strShuffled = ExtRandom<string>.Shuffle(strChoices);
			Debug.Log("Shuffled: " + strShuffled[0].ToString() + "  ,  " + strShuffled[1].ToString() + "  ,  " + strShuffled[2].ToString() + "  ,  " + strShuffled[3].ToString() + "  ,  " + strShuffled[4].ToString() + "  ,  " + strShuffled[5].ToString());
		}

		private void ShuffleTestList()
		{
			List<string> strChoices = new List<string>(6) { "who", "what", "where", "when", "why", "how" };
			List<string> strShuffled = ExtRandom<string>.Shuffle(strChoices);
			Debug.Log("Shuffled: " + strShuffled[0].ToString() + "  ,  " + strShuffled[1].ToString() + "  ,  " + strShuffled[2].ToString() + "  ,  " + strShuffled[3].ToString() + "  ,  " + strShuffled[4].ToString() + "  ,  " + strShuffled[5].ToString());
		}
	}
}

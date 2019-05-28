using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class algorithm : MonoBehaviour
{
    public Button button;
    public Text textA;
    public Text textP;
    public Text textMidOutPut;
    public Text textResult;
    public GameObject answers;
    // Start is called before the first frame update
    void Start()
    {
        ModPAlgorithm(315,907);
    }


    BigInteger ModPAlgorithm(int a,int p)
    {
        /*Step1: p - 1 = 2^t * s
         * input: p
         * output: t,s
         */
        int t = 1; //t>=1
        int s; //s为奇数
        while (true)
        {
            if (IsOdd((p - 1) / (int)Mathf.Pow(2,t)))
            {
                s = (p - 1) / (int)Mathf.Pow(2, t);
                break;
            }
            t = t + 1;
        }

        /*Step2: 计算Xt-1 = a^((s+1)/2)(mod p)
         * input: a,s,p
         * output: Xt-1
         */
        BigInteger[] X = new BigInteger[t];
        X[t - 1] = BigInteger.ModPow((BigInteger)a, (BigInteger)((s + 1) / 2), (BigInteger)p);
        textMidOutPut.text = "X" + (t-1) + " = " + X[t - 1].ToString() + " (mod " + p + ")";

        /*Step3: 如果t=1,则X[t-1] = X[0]是同余式的解 
         * input: X[t-1]
         * output: X[0]
         */
        if (t == 1)
        {
            Debug.Log(X[t - 1]);
            textResult.text = "x = " + X[0] + " (mod " + p + ")";
            return X[t - 1];
        }

        /*Step4: 求出模p的所有二次非剩余
         * input: p
         * output: p的二次非剩余List
         */
        List<int> QuadraticResidueList = new List<int>();
        List<int> QuadraticNonResidueList = new List<int>();
        for (int i = 1; i <= (p - 1) / 2; i++)
        {
            int newQuadraticResidue = (i * i) % p;
            QuadraticResidueList.Add(newQuadraticResidue);
        }
        QuadraticResidueList.Sort();
        for (int i = 1; i <= p; i++)
        {
            if (!QuadraticResidueList.Contains(i))
            {
                QuadraticNonResidueList.Add(i);
            }
        }

        /*Step5 随便选取一个p的二次非剩余
         * input: p的二次非剩余List
         * output: p的一个二次非剩余n
         */
        int n = QuadraticNonResidueList[0];

        /*Step6 计算b = n^s(mod p)
         * input: s，p，p的一个二次非剩余n 
         * output: b
         */
        BigInteger b = BigInteger.ModPow(n, s, p);

        /*Step7 求a模p的逆元
         * input: a
         * output: a的逆元
         */
        BigInteger AInverse = CalculateInverse(a, p);

        /*Step8 求同余式的解
         * input: a的逆元，X[t-1],b,p
         * output: 同余式的解
         */
        int index = 0;
        while (true)
        {
            BigInteger tmp = BigInteger.ModPow(AInverse * (BigInteger.Pow(X[t - 1 - index], 2)), BigInteger.Pow(2, t - 2 - index), p);
            if (tmp == 1)
            {
                X[t - index - 2] = X[t - index - 1];
            }
            else
            {
                X[t - index - 2] = BigInteger.ModPow(X[t - index - 2] = X[t - index - 1] * BigInteger.Pow(b, (int)BigInteger.Pow(2, index)), 1, p);
            }
            index = index + 1;
            textMidOutPut.text = textMidOutPut.text + "\n" + "X" + (t - 1 - index) + " = " + X[t - 1 - index].ToString() + " (mod " + p + ")";
            if (t- index - 1 == 0)
            {
                textResult.text = "x = " + X[0] + " (mod " + p + ")";
                Debug.Log(X[0]);
                break;
            }
        }
        return X[0];
    }

    //判断是否为奇数
    private bool IsOdd(int num)
    {
        return (num & 1) == 1;
    }

    //判断是否为素数
    public bool IsPrimeNum(int num)
    {
        return true;
    }

    //辗转相除法求a模p的逆元(暂时没用上）
    private int CalculateInverseDivision(int x,int y)
    {
        int a, b, n;
        if (x > y)
        {
            a = x;
            b = y;
        }
        else
        {
            a = y;
            b = x;
        }
        int[] r = new int[a];
        int[] q = new int[a];
        r[0] = a % b;
        q[0] = a / b;
        r[1] = b % r[0];
        q[1] = b / r[0];
        int i = 2;
        while (true)
        {
            r[i] = r[i - 2] % r[i - 1];
            q[i] = r[i - 2] / r[i - 1];
            if (r[i] == 0)
            {
                n = i - 1;
                break;
            }
        }
        for(int j = n; j>=0; j--)
        {
            //TODO
        }
        return 1;
    }

    //枚举法求a模p的逆元
    private int CalculateInverse(int a,int p)
    {
        for(int i = 1; i < 999999; i++)
        {
            if (BigInteger.ModPow(a * i, 1, p) == 1)
            {
                return i;
            }
        }
        return 0;
    }

   public void StartCalculate()
   {
        answers.SetActive(true);
        if(textA.text == "" && textP.text =="")
        {
            Debug.Log("输入a或p为空！");
            textResult.text = "输入a或p为空！";
        }
        int a = System.Convert.ToInt32(textA.text);
        int p = System.Convert.ToInt32(textP.text);
        ModPAlgorithm(a, p);
    }

    public void Exit()
    {
        Application.Quit();
    }
}

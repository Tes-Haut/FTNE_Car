using System;
using System.Linq;
using UnityEngine;
public class Individu : MonoBehaviour
{
    [SerializeField] private CarMovement carMovement;
    
    public float[,] w_i_h = new float[7, 6];
    public float[] b_i_h = new float[6];
    public float[,] w_h_o = new float[6, 3];
    public float[] b_h_o = new float[3];

    public float fitness { get; private set; } = 0;
    public bool isDead = false;
    
    private System.Random random = new System.Random();

    float Sigmoid(float x)
    {
        return 1.0f / (1.0f + Mathf.Exp(-x));
    }

    public void SetIsDead()
    {
        isDead = true;
        carMovement.isDead = true;
        Debug.Log("Mort");
    }
    private float GetRandomFloat(float minValue, float maxValue)
    {
        double randomDouble = random.NextDouble();
        float result = (float)(minValue + (randomDouble * (maxValue - minValue)));
        return result;
    }

    public Individu CrossOver(Individu individu, Individu prefab)
    {
        Individu newIndividu = Instantiate(prefab);
        
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                newIndividu.w_i_h[i, j] = (w_i_h[i,j] + individu.w_i_h[i,j] ) /2f;
                if (i == 0)
                    newIndividu.b_i_h[j] = (b_i_h[j] + individu.b_i_h[j] ) /2f;
            }
        }

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                newIndividu.w_h_o[i, j] = (w_h_o[i,j] + individu.w_h_o[i,j] ) /2f;
                if (i == 0)
                    newIndividu.b_h_o[j] = (b_h_o[j] + individu.b_h_o[j] ) /2f;
            }
        }
        newIndividu.PertubWeights();
        
        return newIndividu;
    }
    
    public void SetNN()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                w_i_h[i, j] = (GetRandomFloat(-0.5f, 0.5f));
                if (i == 0)
                    b_i_h[j] = (GetRandomFloat(-0.5f, 0.5f));
            }
        }

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                w_h_o[i, j] = (GetRandomFloat(-0.5f, 0.5f));
                if (i == 0)
                    b_h_o[j] = (GetRandomFloat(-0.5f, 0.5f));
            }
        }
    }

    public void PertubWeights()
    {
        float randomDouble = GetRandomFloat(0f, 4f);

        if (randomDouble < 1f)
        {
            for (int i = 0; i < w_i_h.GetLength(0); i++)
            {
                for (int j = 0; j < w_i_h.GetLength(1); j++)
                    w_i_h[i, j] += GetRandomFloat(-0.3f, 0.3f);
            }
            
        }
        else if (randomDouble < 2f)
        {
            for (int i = 0; i < w_i_h.GetLength(1); i++)
                b_i_h[i] += GetRandomFloat(-0.3f, 0.3f);
        }
        else if (randomDouble < 3f)
        {
            for (int i = 0; i < w_h_o.GetLength(0); i++)
            {
                for (int j = 0; j < w_h_o.GetLength(1); j++)
                    w_h_o[i, j] += GetRandomFloat(-0.3f, 0.3f);
            }
        }
        else if (randomDouble < 4f)
        {
            for (int i = 0; i < w_h_o.GetLength(1); i++)
                b_h_o[i] += GetRandomFloat(-0.3f, 0.3f);
        }
    }
    
    public void ForwardPropagation()
    {
        if (isDead = carMovement.isDead) return;

        fitness += carMovement.speed * Time.deltaTime * 10f;

        float[] _inputs = carMovement.returnRayCast();
        int colsHidden = w_i_h.GetLength(1);

        float[] resultHidden = new float[colsHidden];
        for (int j = 0; j < colsHidden; j++)
        {
            for (int i = 0; i < w_i_h.GetLength(0); i++)
            {
                resultHidden[j] += _inputs[i] * w_i_h[i, j];
            }
            resultHidden[j] += b_i_h[j];
            resultHidden[j] = Sigmoid(resultHidden[j]);
        }

        int colsOutputs = w_h_o.GetLength(1);
        float[] outputResult = new float[colsOutputs];

        for (int j = 0; j < colsOutputs; j++)
        {
            for (int i = 0; i < w_h_o.GetLength(0); i++)
            {
                outputResult[j] += resultHidden[i] * w_h_o[i, j];
            }
            outputResult[j] += b_h_o[j];
            outputResult[j] = Sigmoid(outputResult[j]);
        }

        int maxIndex = Array.IndexOf(outputResult, outputResult.Max());
        carMovement.Action(maxIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7 && carMovement.speed > 0f)
            fitness += 20f;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 0)
            fitness -= 50f;
    }
}
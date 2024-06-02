using System.Linq;
using UnityEngine;

public class Population : MonoBehaviour
{
    [SerializeField] private Individu prefab;
    [SerializeField] private Collider[] gate;
    private const int NbrIndividu = 150;
    private Individu[] population = new Individu[NbrIndividu];

    private void Start()
    {
        for (int i = 0; i < NbrIndividu; i++)
        {
            population[i] = Instantiate(prefab);
            population[i].SetNN();
        }
    }

    private void Update()
    {
        bool allDead = true;


        for (int i = 0; i < NbrIndividu; i++)
        {
            population[i].ForwardPropagation();
            if (!population[i].isDead)
                allDead = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            allDead = true;
            for (int i = 0; i < NbrIndividu; i++)
                population[i].SetIsDead();
        }

        if (!allDead) return;

        System.Array.Sort(population, (a, b) => b.fitness.CompareTo(a.fitness));

        int start = 4, end = 20;

        for (int i = 0; i < start; i++)
        {
            float[] bho = (float[])population[i].b_h_o.Clone();
            float[] bih = (float[])population[i].b_i_h.Clone();

            float[,] who = (float[,])population[i].w_h_o.Clone();
            float[,] wih = (float[,])population[i].w_i_h.Clone();
            
            Destroy(population[i].gameObject);
            
            population[i] = Instantiate(prefab);
            population[i].b_h_o = bho;
            population[i].b_i_h = bih;
            population[i].w_h_o = who;
            population[i].w_i_h = wih;
        }
            
        for (int i = start; i < NbrIndividu; i++)
            Destroy(population[i].gameObject);

        for (int i = start; i < NbrIndividu - end - 50; i++)
        {
            population[i] = population[i % 2].CrossOver(population[i % 2 + 2], prefab);
        }
        for (int i = NbrIndividu - end - 50; i < NbrIndividu - end; i++)
        {
            population[i] = Instantiate(prefab);

            population[i].b_h_o = (float[])population[i % start].b_h_o.Clone();
            population[i].b_i_h = (float[])population[i % start].b_i_h.Clone();
            population[i].w_h_o = (float[,])population[i % start].w_h_o.Clone();
            population[i].w_i_h = (float[,])population[i % start].w_i_h.Clone();

            population[i].PertubWeights();
        }
        for (int i = NbrIndividu - end; i < NbrIndividu; i++)
        {
            population[i] = Instantiate(prefab);
            population[i].SetNN();
        }

        /*Individu[] newPop = new Individu[NbrIndividu];
        {
            for (int i = 0; i < 10; i++)
            {
                newPop[i] = Instantiate(prefab);

                float[] bho = (float[])population[i].b_h_o.Clone();
                float[] bih = (float[])population[i].b_i_h.Clone();

                float[,] who = (float[,])population[i].w_h_o.Clone();
                float[,] wih = (float[,])population[i].w_i_h.Clone();

                newPop[i].b_h_o = bho;
                newPop[i].b_i_h = bih;
                newPop[i].w_h_o = who;
                newPop[i].w_i_h = wih;
            }
        }

        for (int i = 10; i < NbrIndividu - 20; i++)
        {
            float[] bho = (float[])newPop[i % 10].b_h_o.Clone();
            float[] bih = (float[])newPop[i % 10].b_i_h.Clone();

            float[,] who = (float[,])newPop[i % 10].w_h_o.Clone();
            float[,] wih = (float[,])newPop[i % 10].w_i_h.Clone();

            newPop[i] = Instantiate(prefab);

            newPop[i].b_h_o = bho;
            newPop[i].b_i_h = bih;
            newPop[i].w_h_o = who;
            newPop[i].w_i_h = wih;

            newPop[i].pertubWeights();
        }

        for (int i = NbrIndividu - 20; i < NbrIndividu; i++)
        {
            newPop[i] = Instantiate(prefab);
            newPop[i].SetNN();
        }

        foreach (var individu in population)
            Destroy(individu.gameObject);

        population = newPop;*/
    }
}
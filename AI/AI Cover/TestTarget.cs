using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTarget : MonoBehaviour
{
    public bool attacking;
    public Vector3 averageVector;
    public List<Vector3> vectors;
    public float timer;
    public float maxtime;
    public float speed;
    public bool fired;
    public Vector3 sum;
    public Vector3 averagevelocity;
    public List<Vector3> vectors2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(vectors.Count!=5)
        {

            sum = Vector3.zero;
            if (timer < maxtime)
                {
                    timer += speed * Time.deltaTime;
                }
                else
                {
                timer = 0;
                if(vectors.Count>0)
                {
                        vectors.Add(transform.position );
                }
                else
                {
                    vectors.Add(transform.position);

                }
            }
            
        }
        else
        {
            for (int i = 0; i < vectors.Count; i++)
            {
                if (i > 0)
                {
                    sum += (vectors[i] - vectors[i - 1]);
                    //sum += (vectors[i]);
                    vectors2.Add((vectors[i]));
                    //vectors2.Add(vectors[i]);

                }
                else
                {
                    //sum += vectors[i];
                    vectors2.Add(vectors[i]);
                }
            }
            averagevelocity = (sum);
            averageVector = (transform.position+sum);
            //averageVector = vectors[4];
            vectors.Clear();
            
            fired = false;



        }
    }
}

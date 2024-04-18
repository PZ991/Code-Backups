using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class AIEyesHead : MonoBehaviour
{
    public AIFighterAI fightAI;
    public bool NeedsEye;
    public List<Eye> Eye;
    public Eye selectedEye;

    public float radius;
    public float angle;


    public LayerMask obstacle;
    public LayerMask targets;
    public Transform radial;
    public Transform radial2;

    public List<bool> boolcheck;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRadials();
        VisibleTarget();
    }
    private void OnDrawGizmos()
    {
        if(NeedsEye==false)
        Gizmos.DrawWireSphere(transform.position, radius);
        else
            Gizmos.DrawWireSphere(selectedEye.transform.position, selectedEye.radius);

        //Vector3 Vangle = DirectionfromAngle(-angle / 2, false);
        //Vector3 Vangle2 = DirectionfromAngle(angle / 2, false);


        //Gizmos.DrawLine(Eye.position, Vangle * radius);
        //Gizmos.DrawLine(Eye.position, Vangle2 * radius);
    }
    public void VisibleTarget()
    {
        if (NeedsEye)
        {
            fightAI.possibletargets.Clear();



            Collider[] targetsinview = Physics.OverlapSphere(selectedEye.transform.position, selectedEye.radius, targets);

            for (int i = 0; i < targetsinview.Length; i++)
            {
                Transform target = targetsinview[i].transform;

                Vector3 dirTotarget = (target.position - selectedEye.transform.position).normalized;




                if (Vector3.Angle(selectedEye.transform.forward, dirTotarget) < selectedEye.angle / 2)
                {
                    float dstTotarget = Vector3.Distance(selectedEye.transform.position, target.position);
                    if (!Physics.Raycast(selectedEye.transform.position, dirTotarget, dstTotarget, obstacle))
                    {
                        Debug.DrawLine(selectedEye.transform.position, target.position, Color.red);
                        if (!(fightAI.possibletargets.Contains(target.gameObject)))
                        {
                            fightAI.possibletargets.Add(target.gameObject);
                        }
                    }
                }



            }
        }
        else
        {
            fightAI.possibletargets.Clear();



            Collider[] targetsinview = Physics.OverlapSphere(transform.position, radius, targets);

            for (int i = 0; i < targetsinview.Length; i++)
            {
                Transform target = targetsinview[i].transform;

                Vector3 dirTotarget = (target.position - transform.position).normalized;




                if (Vector3.Angle(transform.forward, dirTotarget) < angle / 2)
                {
                    float dstTotarget = Vector3.Distance(transform.position, target.position);
                    if (!Physics.Raycast(transform.position, dirTotarget, dstTotarget, obstacle))
                    {
                        Debug.DrawLine(transform.position, target.position, Color.red);
                        if (!(fightAI.possibletargets.Contains(target.gameObject)))
                        {
                            fightAI.possibletargets.Add(target.gameObject);
                        }
                    }
                }



            }
        }
        
    }
    public Vector3 DirectionfromAngle(float angledegrees, bool global)
    {
        if (!global)
        {
            angledegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angledegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angledegrees * Mathf.Deg2Rad));
    }
    public void UpdateRadials()
    {
        if (NeedsEye)
        {
            if (radial != null)
            {
                radial.position = selectedEye.transform.position;
                float lerpradius = Mathf.InverseLerp(0, 360, selectedEye.angle);
                radial.GetComponent<Image>().fillAmount = lerpradius;
                radial.eulerAngles = new Vector3(90 + selectedEye.transform.eulerAngles.z, selectedEye.transform.eulerAngles.y - selectedEye.angle / 2, selectedEye.transform.eulerAngles.x);
            }
            if (radial2 != null)
            {
                radial2.position = selectedEye.transform.position;
                float lerpradius2 = Mathf.InverseLerp(0, 360, selectedEye.angle);
                radial2.GetComponent<Image>().fillAmount = lerpradius2;
                radial2.eulerAngles = new Vector3(selectedEye.transform.eulerAngles.z, 90 + selectedEye.transform.eulerAngles.y, (selectedEye.transform.eulerAngles.x + selectedEye.angle / 2) + 90);
            }
        }
        else
        {
            if (radial != null)
            {
                radial.position = transform.position;
                float lerpradius = Mathf.InverseLerp(0, 360, angle);
                radial.GetComponent<Image>().fillAmount = lerpradius;
                radial.eulerAngles = new Vector3(90 + transform.eulerAngles.z, transform.eulerAngles.y - angle / 2, transform.eulerAngles.x);
            }
            if (radial2 != null)
            {
                radial2.position = transform.position;
                float lerpradius2 = Mathf.InverseLerp(0, 360, angle);
                radial2.GetComponent<Image>().fillAmount = lerpradius2;
                radial2.eulerAngles = new Vector3(transform.eulerAngles.z, 90 + transform.eulerAngles.y, (transform.eulerAngles.x + angle / 2) + 90);
            }
        }
    }

    
}

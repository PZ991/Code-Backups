using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intercept1 : MonoBehaviour
{
    public float shotspeed;
    public GameObject shooter;
    public GameObject target;
    public Vector3 intercept;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //intercept = CalculateInterceptCourse(target.transform.position, target.GetComponent<Rigidbody>().velocity, transform.position, shotspeed);
       StartCalculation();
        //Debug.DrawRay(transform.position, intercept);
        //transform.position = Vector3.MoveTowards(transform.position, intercept, shotspeed * Time.deltaTime);
        
    }
    #region V1
    public Vector3 CalculateInterceptCourse(Vector3 targetpos, Vector3 targetspeed,Vector3 Interceptorpos, float Interceptorspeed)
    {
        Vector3 targetdir = targetpos - Interceptorpos;
        float ispeed2 = Interceptorspeed * Interceptorspeed;
        float tspeed = targetspeed.sqrMagnitude;
        float fDot1 = Vector3.Dot(targetdir, targetspeed);
        float targetdist2 = targetdir.sqrMagnitude;
        float d = (fDot1 * fDot1) - targetdist2 * (tspeed- ispeed2);
        if (d < 0.1f)           //no possible course, not fast enough

            return Vector3.zero;
            //return targetpos;
        
        float sqrt = Mathf.Sqrt(d);
        float s1 = (-fDot1 - sqrt) / targetdist2;
        float s2 = (-fDot1 + sqrt) / targetdist2;
        if(s1<0.0001f)
        {
            if (s2 < 0.0001f)
                return Vector3.zero;
                //return targetpos;
            else
                return s2 * targetdir + targetspeed;
        }
        else if (s2 < 0.0001f)
        {
            return (s1) * targetdir + targetspeed;
        }
        else if (s1<s2)
        {
            return (s2) * targetdir + targetspeed;
        }
        else
            return (s1) * targetdir + targetspeed;
    }
    #endregion


    #region V2
    public void StartCalculation()
    {
        Vector3 shooterPosition = shooter.transform.position;
        Vector3 targetPosition = target.transform.position;
        Vector3 shootervelocity = shooter.GetComponent<Rigidbody>() ? shooter.GetComponent<Rigidbody>().velocity : Vector3.zero;
        Vector3 targetvelocity = target.GetComponent<Rigidbody>() ? target.GetComponent<Rigidbody>().velocity : Vector3.zero;

        Vector3 interceptPoint = FirstOrderIntercept(shooterPosition, shootervelocity, shotspeed, targetPosition, targetvelocity);
        Debug.DrawRay(transform.position, interceptPoint);
        if(Input.GetKeyDown(KeyCode.M))
        transform.GetComponent<Rigidbody>().AddForce(interceptPoint - transform.position * shotspeed,ForceMode.Acceleration);
    }

    public static Vector3 FirstOrderIntercept(Vector3 shooterpos,Vector3 shootervel,float shotspeed,Vector3 targetpos,Vector3 targetvel)
    {
        Vector3 targetrelativepos = targetpos - shooterpos;
        Vector3 targetrelativevel = targetvel - shootervel;
        float t = FirstOrderInterceptTime(shotspeed, targetrelativepos, targetrelativevel);
        return targetpos + t * (targetrelativevel);

    }

    public static float FirstOrderInterceptTime(float shotspeed, Vector3 targetrelpos, Vector3 targetrelvel)
    {
        float velocitysquared = targetrelvel.sqrMagnitude;
        if (velocitysquared < 0.001f)
            return 0;
        float a = velocitysquared - shotspeed * shotspeed;
        if(Mathf.Abs(a)<0.001f)
        {
            float t = -targetrelpos.sqrMagnitude / (2f * Vector3.Dot(targetrelvel, targetrelpos));
            return Mathf.Max(t, 0);
        }
        float b = 2f * Vector3.Dot(targetrelvel, targetrelpos);
        float c = targetrelpos.sqrMagnitude;
        float determinant = b * b - 4f * a * c;
        if (determinant > 0)
        {
            float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a);
            float t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);
            if (t1 > 0)
            {
                if (t2 > 0)
                    return Mathf.Min(t1, t2);
                else
                    return t1;
            }
            else
                return Mathf.Max(t2, 0f);
        }
        else if (determinant < 0)
            return 0;
        else
            return Mathf.Max(-b / (2f * a), 0);
    }
    #endregion
}

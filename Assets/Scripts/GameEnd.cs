using UnityEngine;

public class GameEnd : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            UnityEngine.Debug.Log(1);
            Application.Quit(0);
        }
    }
}

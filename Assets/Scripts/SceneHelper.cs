using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHelper : MonoBehaviour
{   

   public void ActivateDreamscape(int scapeNum){
       Debug.Log("trying to activate level " + scapeNum);
       if(scapeNum == 2)
       {
            SceneManager.LoadScene(1);
       } 
       else if (scapeNum == 3)
       {
           SceneManager.LoadScene(2);
       } 
       else if (scapeNum == 4)
        {
            SceneManager.LoadScene(3);
        }   
   }
}

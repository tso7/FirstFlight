using UnityEngine;
using System.Collections;

public class ExplosionController : GenericSingleton<ExplosionController> {

    public GameObject[] explosion;
    private bool isactive_ = false;
    private GameObject exp;

    public void Explode (Vector3 position) {
        if (explosion != null && !isactive_) {
            int index = Random.Range((int)0, (int)explosion.Length - 1);
            
            exp = Instantiate(explosion[index], position, Quaternion.identity) as GameObject;
            
            StartCoroutine(DeadTime());
        }
    }

    IEnumerator DeadTime () {
        isactive_ = true;
        yield return new WaitForSeconds(2f);
        Destroy(exp);
        isactive_ = false;
    }
}

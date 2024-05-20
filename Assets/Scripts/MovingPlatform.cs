using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;//vi tri A
    public Transform pointB;//vi tri B
    public float moveSpeed = 2f;
    private Vector3 nextPosition;//vi tri tiep theo de platfrom di chuyen den
    private bool canChange;



    // Start is called before the first frame update
    void Start()
    {
        GameController.OnReset +=ResetPosition;
        nextPosition = pointB.position;
        canChange = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);
        if (transform.position == nextPosition && canChange)
        {
            StartCoroutine(ChangePosition());
        }
    }

    private IEnumerator ChangePosition()
    {
        canChange = false;

        yield return new WaitForSeconds(1f);
        nextPosition = nextPosition == pointB.position ? pointA.position : pointB.position;
        canChange = true;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.activeInHierarchy)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.transform.parent = transform;//neu nhan vat dang dung tren platform thi xet nhan vat lam con cua platform
                //dieu nay de nhan vat di chuyen cung platform luon, khong bi truot ra khoi platform
            }
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if(gameObject.activeInHierarchy)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.transform.parent = null;//neu thoat collision thi xet lai thanh null de doc lap position
            }
        }
    }

    void ResetPosition()
    {
        canChange = true;
    }

}

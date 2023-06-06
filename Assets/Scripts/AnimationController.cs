using UnityEngine;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour
{
    public Animator anim; // �ִϸ����� ������Ʈ�� �����ϱ� ���� ����
    public bool isjump = false;

    private void Start()
    {
        //anim.GetComponent<Animator>();
        anim.SetBool("JUMP", false);
    }

    public void ChangeAnimation()
    {
        if (isjump == true)
        {
            anim.SetBool("JUMP", false);
            isjump = false;
        }

        else
        {
            anim.SetBool("JUMP", true);
            isjump = true;
        }
    }
}

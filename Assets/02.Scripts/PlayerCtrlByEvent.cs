using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrlByEvent : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction attackAction;

    private Animator anim;
    private Vector3 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        //Move 액션 생성 및 타입 설정
        moveAction = new InputAction("Move", InputActionType.Value);

        //Move 액션의 복합 바인딩 정보 정의
        moveAction.AddCompositeBinding("2DVector")
        .With("Up", "<Keyboard>/w")
        .With("Down", "<Keyboard>/s")
        .With("Left", "<Keyboard>/a")
        .With("Right", "<Keyboard>/d");

        //Move 액션의 performed, canceled 이벤트 연결
        moveAction.performed += ctx => {
            Vector2 dir = ctx.ReadValue<Vector2>();
            moveDir = new Vector3(dir.x, 0, dir.y);
            //Warrior_Run 애니메이션 실행
            anim.SetFloat("Movement", dir.magnitude);               
        };

        moveAction.canceled += ctx => {
            moveDir = Vector3.zero;
            anim.SetFloat("Movement", 0.0f);
        };

        //Move 액션의 활성화
        moveAction.Enable();

        //Attack 액션 생성
        attackAction = new InputAction("Attack",
                                       InputActionType.Button,
                                       "<Keyboard>/space");
                                       
        //Attack 액션의 performed 이벤트 연결
        attackAction.performed += ctx => {
            anim.SetTrigger("Attack");
        };
        //Attack 액션의 활성화
        attackAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveDir != Vector3.zero)
        {
            //진행 방향으로 회전
            transform.rotation = Quaternion.LookRotation(moveDir);
            //회전한 후 전진방향으로 이동
            transform.Translate(Vector3.forward * Time.deltaTime * 4.0f);
        }        
    }
}

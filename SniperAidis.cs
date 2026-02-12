using UnityEngine;
using System.Collections;

public class SniperAids : MonoBehaviour
{
    public Camera playerCam;
    public GameObject scopeOverlay;

    public float zoomFOV = 20f;
    public float normalFOV = 60f;
    public float zoomSpeed = 10f;

    [Header("Recoil")]
    public float recoilAmount = 5f;      // Quanto a câmera sobe
    public float recoilSpeed = 10f;      // Velocidade de subir
    public float returnSpeed = 5f;       // Velocidade de voltar

    private float currentRecoil = 0f;
    private float targetRecoil = 0f;

    bool isAiming = false;

    void Update()
    {
        // MIRAR
        if (Input.GetMouseButtonDown(1))
        {
            isAiming = true;
            scopeOverlay.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(ChangeFOV(zoomFOV));
        }

        if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            scopeOverlay.SetActive(false);
            StopAllCoroutines();
            StartCoroutine(ChangeFOV(normalFOV));
        }

        // ATIRAR
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        HandleRecoil();
    }

    void Shoot()
    {
        // Aplica recoil
        targetRecoil += recoilAmount;
    }

    void HandleRecoil()
{
    // Interpola subida
    currentRecoil = Mathf.Lerp(currentRecoil, targetRecoil, Time.deltaTime * recoilSpeed);

    // Interpola volta
    targetRecoil = Mathf.Lerp(targetRecoil, 0, Time.deltaTime * returnSpeed);

    // Aplica recoil SOMANDO ao eixo X atual
    Vector3 camRot = playerCam.transform.localEulerAngles;
    camRot.x -= currentRecoil;
    playerCam.transform.localEulerAngles = camRot;
}


    IEnumerator ChangeFOV(float target)
    {
        while (Mathf.Abs(playerCam.fieldOfView - target) > 0.1f)
        {
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, target, Time.deltaTime * zoomSpeed);
            yield return null;
        }

        playerCam.fieldOfView = target;
    }
}

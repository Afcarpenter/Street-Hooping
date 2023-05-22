using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public float maxThrowForce;
    public float minThrowForce;
    public float maxThrowForceChargeTime;
    public float ballSpawnDelay;
    public GameObject basketballPrefab;
    public Transform basketballHoldPoint;
    public Slider chargeSlider;

    private float throwForce;
    private float chargeSpeed;
    private Vector3 throwVelocity;
    private bool gameIsActive = false;

    private GameObject mainCamera;
    private GameObject heldBasketball;
    private TrajectoryLine trajectoryLine;

    // Start is called before the first frame update
    void Start()
    {
        chargeSpeed = (maxThrowForce - minThrowForce) / maxThrowForceChargeTime;
        throwForce = minThrowForce;

        mainCamera = GameObject.Find("Main Camera");
        trajectoryLine = GetComponent<TrajectoryLine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (heldBasketball != null && gameIsActive)
        {
            StopAllCoroutines();

            if ((Input.GetButtonUp("Fire1") || throwForce >= maxThrowForce))
            {
                if (throwForce > maxThrowForce)
                {
                    throwForce = maxThrowForce;
                }

                throwVelocity = (mainCamera.transform.forward + Vector3.up).normalized * throwForce;

                trajectoryLine.ShowTrajectoryLine(basketballHoldPoint.position, throwVelocity);
                ThrowBall(throwVelocity);
                StartCoroutine(HideShotUI());

                throwForce = minThrowForce;
            } else if (Input.GetButtonDown("Fire1"))
            {
                //Make the force slider appear and set its value to throwforce
                chargeSlider.gameObject.SetActive(true);
                chargeSlider.value = (throwForce - minThrowForce) / (maxThrowForce - minThrowForce);

            } else if (Input.GetButton("Fire1"))
            {
                throwForce += chargeSpeed * Time.deltaTime;
                throwVelocity = (mainCamera.transform.forward + Vector3.up).normalized * throwForce;

                chargeSlider.gameObject.SetActive(true);
                trajectoryLine.ShowTrajectoryLine(basketballHoldPoint.position, throwVelocity);
                chargeSlider.value = (throwForce - minThrowForce) / (maxThrowForce - minThrowForce);
            }
        }
    }

    void ThrowBall(Vector3 velocity)
    {
        //Make the ball have gravity, send it on its trajectory, and reset the ball
        Rigidbody ballRB = heldBasketball.GetComponent<Rigidbody>();
        ballRB.useGravity = true;
        ballRB.AddForce(velocity, ForceMode.Impulse);
        heldBasketball.GetComponent<Basketball>().PrimeDestroyBall();
        heldBasketball = null;
        throwForce = minThrowForce;
        StartCoroutine(SpawnNextBall());
    }

    IEnumerator SpawnNextBall()
    {
        yield return new WaitForSeconds(ballSpawnDelay);
        SpawnBall();
    }

    public void SpawnBall()
    {
        heldBasketball = Instantiate(basketballPrefab, basketballHoldPoint.position, basketballPrefab.transform.rotation);
    }

    IEnumerator HideShotUI()
    {
        yield return new WaitForSeconds(ballSpawnDelay);
        trajectoryLine.HideTrajectoryLine();
        chargeSlider.gameObject.SetActive(false);
    }

    void HideShotMarker()
    {
        trajectoryLine.HideTrajectoryLine();
        chargeSlider.gameObject.SetActive(false);
    }

    public void ToggleGameActive(bool status)
    {
        gameIsActive = status;
    }
}

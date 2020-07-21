using UnityEngine;
using UnityEngine.UI;

public class PowerupMeterController : MonoBehaviour
{
    public Image attackFill, shieldFill, timeFill, scoreFill;

    private PlayerBoons boons;

    private void Start() {
        boons = PlayerBoons.instance;
    }

    private void Update() {
        attackFill.fillAmount = boons.GetTimeLeftPercentage(PowerupType.Attack);
        shieldFill.fillAmount = boons.GetTimeLeftPercentage(PowerupType.Defense);
        timeFill.fillAmount = boons.GetTimeLeftPercentage(PowerupType.Time);
        scoreFill.fillAmount = boons.GetTimeLeftPercentage(PowerupType.Score);
    }
}
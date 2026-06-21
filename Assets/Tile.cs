using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    public TileType type;
    public GameObject destroyEffectPrefab;
    public BonusType bonusType = BonusType.None;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void OnMouseDown()
    {
        InputManager.instance.HandleTileClick(this);
    }

    public void SetType(TileType newType, Color color)
    {
        type = newType;
        GetComponent<SpriteRenderer>().color = color;
    }

    public void Select()
    {
        transform.localScale = originalScale * 1.2f;
    }

    public void ResetScale()
    {
        transform.localScale = originalScale;
    }
    public void DestroyWithAnimation()
    {
        if (destroyEffectPrefab != null)
        {
            GameObject effect = Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 2f); 
        }

        StartCoroutine(DestroyRoutine());
    }

    private IEnumerator DestroyRoutine()
    {
        Vector3 startScale = transform.localScale;
        float time = 0f;
        float duration = 0.5f;
        
        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, time / duration);
            yield return null;
        }

        Destroy(gameObject);
    }
    public IEnumerator MoveTo(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            yield return null;
        }

        transform.position = targetPosition;
    }

    public IEnumerator Bounce()
    {
        Vector3 startScale = transform.localScale;
        Vector3 bigScale = startScale * 1.25f;

        float time = 0f;
        float duration = 0.25f;

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, bigScale, time / duration);
            yield return null;
        }

        time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(bigScale, startScale, time / duration);
            yield return null;
        }
        
        transform.localScale = startScale;
    }
    public void SetBonus(BonusType newBonusType)
    {
        bonusType = newBonusType;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (bonusType == BonusType.RocketHorizontal)
        {
            sr.color = Color.white;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = Vector3.one * 1.4f;
            Debug.Log("Created horizontal rocket");
        }
        else if (bonusType == BonusType.RocketVertical)
        {
            sr.color = Color.cyan;
            transform.rotation = Quaternion.Euler(0, 0, 90);
            transform.localScale = Vector3.one * 1.4f;
            Debug.Log("Created vertical rocket");
        }
        else if (bonusType == BonusType.ColorBomb)
        {
            sr.color = Color.gray;
            transform.localScale = Vector3.one * 1.8f;
            Debug.Log("Created color bomb");
        }
        else if (bonusType == BonusType.Bomb)
        {
            sr.color = Color.black;
            transform.localScale = Vector3.one * 1.5f;
            Debug.Log("Created bomb");
        }
    }
}
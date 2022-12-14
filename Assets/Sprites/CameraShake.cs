using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;//追加
using UnityEngine.SceneManagement;//追加

public class CameraShake : MonoBehaviour
{
    /// <summary>
    /// カメラを揺らす
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="magnitude"></param>
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(DoShake(duration, magnitude));
    }

    /// <summary>
    /// カメラを揺らす処理を行う
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="magnitude"></param>
    /// <returns></returns>
    private IEnumerator DoShake(float duration, float magnitude)
    {
        var pos = transform.localPosition;

        var elapsed = 0f;

        while (elapsed < duration)
        {
            var x = pos.x + Random.Range(-1f, 1f) * magnitude;
            var y = pos.y + Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, pos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = pos;
    }
}

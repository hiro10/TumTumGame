using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ParamsSO : ScriptableObject
{
    [Header("最初に落ちてくるボールの量")]
    public int initBallCount;

    [Header("ボールを消したときの得点")]
    public int ScorePint;

    [Header("ボールの判定距離")]
    public float ballDistance;

    [Header("ボムの爆破範囲")]
    [Range(1,20)]
    public float bomRange;

    [Header("ボムのの出現率")]
    [Range(0, 100)]
    public float bomSpownRange;

    //MyScriptableObjectが保存してある場所のパス
    public const string PATH = "ParamsSO";

    //MyScriptableObjectの実体
    private static ParamsSO _entity;
    public static ParamsSO Entity
    {
        get
        {
            //初アクセス時にロードする
            if (_entity == null)
            {
                _entity = Resources.Load<ParamsSO>(PATH);

                //ロード出来なかった場合はエラーログを表示
                if (_entity == null)
                {
                    Debug.LogError(PATH + " not found");
                }
            }

            return _entity;

        }

    }
}

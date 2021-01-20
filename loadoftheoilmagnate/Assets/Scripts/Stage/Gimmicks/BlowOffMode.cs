namespace OilMagnate.StageScene
{
    /// <summary>
    /// 吹き飛ばし方を決める識別子
    /// Use <see cref="IContactable.BlowOff(UnityEngine.Vector3, BlowMode)"/>
    /// </summary>
    public enum BlowMode
    {
        FromParam,
        FromVelocity,
    }
}
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using UniEasy.ECS;
using UniEasy;
using UniRx;

public class ABDownloadProgressSystem : SystemBehaviour
{
    private IGroup abDownloadProgressComponents;

    public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory, PrefabFactory prefabFactory)
    {
        base.Initialize(eventSystem, poolManager, groupFactory, prefabFactory);
        abDownloadProgressComponents = this.Create(typeof(ABDownloadProgressComponent), typeof(Scrollbar));
    }

    public override void OnEnable()
    {
        base.OnEnable();
        abDownloadProgressComponents.OnAdd().Subscribe(entity =>
        {
            var abDownloadProgressComponent = entity.GetComponent<ABDownloadProgressComponent>();
            var scrollbar = entity.GetComponent<Scrollbar>();
            var progress = 0;
            var contentSize = 0f;
            var downloadedSize = 0f;

            Observable.EveryUpdate().Subscribe(_ =>
            {
                progress = ABLoaderManager.GetDownloadIntProgress();
                contentSize = ABLoaderManager.GetContentSize(0);
                downloadedSize = ABLoaderManager.GetDownloadedSize(0);
                scrollbar.size = 0.01f * progress;
                abDownloadProgressComponent.Progress.text = progress + "%";
                abDownloadProgressComponent.Size.text = contentSize > 0 ? GetSizeByRecursive(contentSize, downloadedSize) : "";
                if (progress >= 100) { SceneManager.LoadScene(1); }
            }).AddTo(this.Disposer).AddTo(abDownloadProgressComponent.Disposer);
        }).AddTo(this.Disposer);
    }

    private string GetSizeByRecursive(float contentSize, float downloadSize, int num = 0)
    {
        var cs = contentSize / 1024f;
        var ds = downloadSize / 1024f;
        if (cs >= 1f)
        {
            return GetSizeByRecursive(cs, ds, ++num);
        }
        else
        {
            return Mathf.RoundToInt(downloadSize) + "/" + Mathf.RoundToInt(contentSize) + GetUnit(num);
        }
    }

    private string GetUnit(int num)
    {
        switch (num)
        {
            case 1:
                return "KB";
            case 2:
                return "MB";
            case 3:
                return "GB";
            case 4:
                return "TB";
            default:
                return "B";
        }
    }
}

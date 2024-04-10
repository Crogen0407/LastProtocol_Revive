using UnityEngine;

public class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private ResourceListSO ResourceList;

    public ResourceSO GetResourceSO(Resource resource)
    {
        return ResourceList.resourceList[(int)resource];
    }
}

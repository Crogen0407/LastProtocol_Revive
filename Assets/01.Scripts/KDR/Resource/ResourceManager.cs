using UnityEngine;

public class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private ResourceListSO ResourceList;

    public ResourceSO GetResourceSO(ResourceType resource)
    {
        return ResourceList.resourceList[(int)resource];
    }
}

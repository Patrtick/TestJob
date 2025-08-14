using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    // Загружаем сцену с индексом 1
    public void LoadSceneWithIndex1()
    {
        SceneManager.LoadScene(1);
    }
}

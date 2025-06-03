using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public GameObject[] cubes;      // 0: 레드, 1: 블루 (색상별 프리팹)
    public Transform[] points;      // 0~3: 스폰 위치(좌표)
    public AudioSource audioForBlock;

    private List<NoteData> noteList = new List<NoteData>();
    private int nextNoteIdx = 0;

    class NoteData
    {
        public float time;
        public int spawnPoint;
        public int direction;
        public int colorIndex;
    }
    
    void Start()
    {
        StartCoroutine(LoadNotesFromCSV());
        nextNoteIdx = 0;
        audioForBlock.Play();
    }

    void Update()
    {
        if (nextNoteIdx >= noteList.Count) return;
        float musicTime = audioForBlock.time;

        // 다음 노트의 타이밍이 지났으면 스폰!
        while (nextNoteIdx < noteList.Count && noteList[nextNoteIdx].time <= musicTime)
        {
            var note = noteList[nextNoteIdx];
            SpawnBlock(note.colorIndex, note.direction, note.spawnPoint);
            nextNoteIdx++;
        }
    }

    void SpawnBlock(int colorIndex, int direction, int spawnPoint)
    {
        // cubes: 색상별 프리팹, points: 위치
        GameObject prefab = cubes[Mathf.Clamp(colorIndex, 0, cubes.Length-1)];
        Transform spawnPos = points[Mathf.Clamp(spawnPoint-1, 0, points.Length-1)]; // CSV 1~4, 배열 0~3
        GameObject block = Instantiate(prefab, spawnPos);
        block.transform.localPosition = Vector3.zero;
        //block.transform.Rotate(transform.forward, 90 * direction);
        block.transform.rotation = GetBlockRotation(direction);
    }


    Quaternion GetBlockRotation(int direction)
    {
        switch (direction)
        {
            case 0: return Quaternion.Euler(0, 0, 0);     // 위
            case 1: return Quaternion.Euler(0, 0, 180);   // 아래
            case 2: return Quaternion.Euler(0, 0, 90);    // 왼쪽
            case 3: return Quaternion.Euler(0, 0, -90);   // 오른쪽
            default: return Quaternion.identity;
        }
    }

    IEnumerator LoadNotesFromCSV()
    {
        string path = System.IO.Path.Combine(Application.streamingAssetsPath, "pattern.csv");
        string csvText = "";

#if UNITY_ANDROID && !UNITY_EDITOR
        UnityWebRequest www = UnityWebRequest.Get(path);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            csvText = www.downloadHandler.text;
        }
        else
        {
            Debug.LogError("CSV 파일 로드 실패: " + www.error);
            yield break;
        }
#else
        csvText = System.IO.File.ReadAllText(path);
#endif

        // csvText 파싱 (한 줄씩 처리)
        noteList.Clear();
        var lines = csvText.Split('\n');
        bool isFirst = true;
        foreach (var line in lines)
        {
            if (isFirst) { isFirst = false; continue; }
            var tokens = line.Trim().Split(',');
            if (tokens.Length < 4) continue;
            NoteData note = new NoteData();
            note.time = float.Parse(tokens[0]);
            note.spawnPoint = int.Parse(tokens[1]);
            note.direction = int.Parse(tokens[2]);
            note.colorIndex = int.Parse(tokens[3]);
            noteList.Add(note);
        }
        Debug.Log("노트 데이터 로드 완료! 총 " + noteList.Count + "개");
        yield break;
    }

    
   
}

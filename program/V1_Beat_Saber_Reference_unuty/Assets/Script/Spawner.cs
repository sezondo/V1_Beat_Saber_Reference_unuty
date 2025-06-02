using UnityEngine;
using System.Collections.Generic;
using System.IO;


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
        LoadNotesFromCSV();
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
        block.transform.rotation = GetBlockRotation(direction);
    }

    Quaternion GetBlockRotation(int direction)
    {
        switch (direction)
        {
            case 0: return Quaternion.Euler(0, 180, 0);     // 위
            case 1: return Quaternion.Euler(0, 180, 180);   // 아래
            case 2: return Quaternion.Euler(0, 180, 90);    // 왼쪽
            case 3: return Quaternion.Euler(0, 180, -90);   // 오른쪽
            default: return Quaternion.identity;
        }
    }

    void LoadNotesFromCSV()
    {
        string path = Path.Combine(Application.dataPath, "pattern.csv");
        if (!File.Exists(path))
        {
            Debug.LogError("CSV 파일이 없습니다: " + path);
            return;
        }

        noteList.Clear();
        using (StreamReader sr = new StreamReader(path))
        {
            bool isFirst = true;
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (isFirst) { isFirst = false; continue; } // 헤더 스킵
                var tokens = line.Split(',');
                if (tokens.Length < 4) continue;
                NoteData note = new NoteData();
                note.time = float.Parse(tokens[0]);
                note.spawnPoint = int.Parse(tokens[1]);
                note.direction = int.Parse(tokens[2]);
                note.colorIndex = int.Parse(tokens[3]);
                noteList.Add(note);
            }
        }
        Debug.Log("노트 데이터 로드 완료! 총 " + noteList.Count + "개");
    }

    
   
}

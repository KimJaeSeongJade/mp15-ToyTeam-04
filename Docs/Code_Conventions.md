# C# 코드 규칙

## 1. 이름 규칙

| 대상 | 표기 | 예시 |
| --- | --- | --- |
| 클래스 | PascalCase | `MonsterVision` |
| 메서드 | PascalCase | `DetectPlayer()` |
| public 필드·프로퍼티 | PascalCase | `public int Hp` |
| private 필드 | camelCase | `private float _moveSpeed` |
| 지역 변수·매개변수 | camelCase | `float distance` |
| 상수 | PascalCase | `private const float MAX_HP = 100f` |
| public 필드 열거형 | camelCase | Enum eMonster |
| private 필드 열거형 | PascalCase | Enum _EMonster |
| 열거형 선언 |  | Enum EMonsterType |
1. 열거형 : 따로 스크립트 설정 후 나중에 스크립트 병합
2. 필드 이름 양식
    1. 체력 → hp
    2. 골드 → gold
    3. 접두사 → (접두사)변수명 (상수는 _ 붙이기)
    4. UI 이름
        1. image → imgHpBar
        2. text → txtHp
        3. button → btnTower
3. 메서드(함수) 이름 양식
    1. (담당)(접두사)(기능)
        1. ex) Player
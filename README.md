## 실행하기 전

### Database 설정
- Database는 **MS-SQL**을 사용합니다.
- `appsettings.json`에서 `ContactDbOption` Section을 수정합니다.


```json
"ContactDbOption": {
    "UseBulkInsert": false,
    "ConnectionString": ""
}
```
| property | type | note |
|----------|------|------|
|`UseBulkInsert`|`bool`|데이터 Insert 시 Bulk Insert 실행 유/무를 설정합니다. MS-SQL에서만 동작합니다. |
|`ConnectionString`|`string`|MS-SQL 연결 문자열을 설정합니다. **해당 부분이 공백일 경우 SQLite memory DB로 동작합니다.** |

<br>

## 실행 방법
- Application을 실행하면 초기화 과정에서 Database를 생성합니다.
- Database가 생성되지 않는 경우 `ConnectionString`을 제거해서 **SQLite memory DB**로 동작할 수 있도록 합니다.

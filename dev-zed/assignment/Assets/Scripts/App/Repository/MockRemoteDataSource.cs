using System.IO;
using System;
using Entity;
using Newtonsoft.Json;
using UniRx;

// 목업 데이터를 제공하는 구현체
// 과제에서는 데이터가 로컬에 존재하기에 이러한 형태로 구현 했습니다.
// 인터페이스는 동일하기에 별도의 remote 데이터가 있을 경우 구현체만 변경하는 것으로 코드 수정이 가능해집니다.
public class MockRemoteDataSource : IRemoteDataSource
{
    public IObservable<Response<Dong[]>> GetDongs()
    {
        return Observable.Defer(() =>
        {
            var jsonString = File.ReadAllText("./Assets/Samples/json/dong.json");
            var obj = JsonConvert.DeserializeObject<Response<Dong[]>>(jsonString);

            return Observable.Return(obj);
        });
    }
}
using System;
using Entity;

// 데이터 fetch를 위한 인터페이스
public interface IRemoteDataSource
{
    IObservable<Response<Dong[]>> GetDongs();
}
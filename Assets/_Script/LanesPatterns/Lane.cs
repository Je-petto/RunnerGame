
//abstract와 유사하지만, 최소한의 것만 유지된 가벼운 버전
// abstract = 변수 선언 가능
// interface = 변수 선언 불가능
// interface 는 생성자(Construct) (불가능)
public interface Lane
{

    public string Name { get; }
    public int MaxLane {get; }

    //초기화 함수
    public void Initialize(int maxlane);

    // Lane 정보를 가져오는 함수
    public int GetNextLane();

}

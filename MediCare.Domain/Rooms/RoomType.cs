namespace MediCare.Domain.Rooms;

public enum RoomType
{
    // Bắt đầu từ 1: giá trị mặc định 0 là "chưa chọn" và bị Enum.IsDefined từ chối
    Consultation = 1,   // phòng khám
    Procedure,          // phòng thủ thuật
    Surgery,            // phòng mổ
    Laboratory,         // phòng xét nghiệm
    Imaging,            // chẩn đoán hình ảnh
    Emergency,          // cấp cứu
    Inpatient           // buồng bệnh
}
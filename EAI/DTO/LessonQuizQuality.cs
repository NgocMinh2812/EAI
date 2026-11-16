using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DTO;

public static class LessonQuizQuality
{
    private static string Normalize(string value) => Regex.Replace(value.Trim().ToLowerInvariant(), @"\s+", " ");

    // Nhận diện cả câu mẫu từng được các phiên bản cũ lưu vào database.
    public static bool IsValidQuestion(CauHoiDTO q) =>
        q != null && !string.IsNullOrWhiteSpace(q.NoiDungCauHoi) && q.NoiDungCauHoi.Trim().Length >= 8 &&
        !Regex.IsMatch(q.NoiDungCauHoi, @"\[(?:Practice|Application|Consolidation|Practical Case)\s*#", RegexOptions.IgnoreCase) &&
        q.DanhSachLuaChon != null && q.DanhSachLuaChon.Count == 4 &&
        q.DanhSachLuaChon.All(o => !string.IsNullOrWhiteSpace(o)) &&
        q.DanhSachLuaChon.Select(o => Normalize(CauHoiDTO.StripOptionPrefix(o))).Distinct().Count() == 4 &&
        q.DapAnDungIndex is >= 0 and <= 3 && !string.IsNullOrWhiteSpace(q.GiaiThichChiTiet);

    public static bool IsValidSet(IReadOnlyCollection<CauHoiDTO>? questions)
    {
        if (questions == null || questions.Count != 10 || !questions.All(IsValidQuestion)) return false;
        // Bỏ số thứ tự khi so sánh để không chấp nhận cùng câu được đánh số lại.
        return questions.Select(q => Regex.Replace(Normalize(q.NoiDungCauHoi), @"^(?:question|câu|cau)?\s*\d+[\s.):-]*", ""))
            .Distinct().Count() == 10;
    }
}

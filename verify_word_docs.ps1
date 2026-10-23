$word = New-Object -ComObject Word.Application
$word.Visible = $false
$word.DisplayAlerts = 0

$file1 = "d:\TDC\HK5\CDPTUD\EAI\TestCases_Sprint1_Sprint2_Minh_HocVien.docx"
$file2 = "d:\TDC\HK5\CDPTUD\EAI\TestCases_Sprint1_Sprint2_Em_GiangVien_Admin.docx"

Write-Host "=== KIỂM TRA FILE 1: MINH (HỌC VIÊN) ==="
$doc1 = $word.Documents.Open($file1)
$pages1 = $doc1.ComputeStatistics(2)
Write-Host "Số trang: $pages1"
Write-Host "Số bảng: $($doc1.Tables.Count)"

$non5ColTables = 0
for ($i = 1; $i -le $doc1.Tables.Count; $i++) {
    $tbl = $doc1.Tables.Item($i)
    $colCount = $tbl.Columns.Count
    # The summary meta table and environment table are not test case tables
    # Check if table has headers matching test case table
    $firstRowText = $tbl.Rows.Item(1).Range.Text
    if ($firstRowText -match "Mã TC") {
        if ($colCount -ne 5) {
            Write-Host "BẢNG $i CÓ $colCount CỘT (KHÁC 5)!"
            $non5ColTables++
        }
    }
}
Write-Host "Bảng test case không đúng 5 cột: $non5ColTables"
$doc1.Close($false)

Write-Host "`n=== KIỂM TRA FILE 2: ÊM (GIẢNG VIÊN / ADMIN) ==="
$doc2 = $word.Documents.Open($file2)
$pages2 = $doc2.ComputeStatistics(2)
Write-Host "Số trang: $pages2"
Write-Host "Số bảng: $($doc2.Tables.Count)"

$non5ColTables2 = 0
for ($i = 1; $i -le $doc2.Tables.Count; $i++) {
    $tbl = $doc2.Tables.Item($i)
    $colCount = $tbl.Columns.Count
    $firstRowText = $tbl.Rows.Item(1).Range.Text
    if ($firstRowText -match "Mã TC") {
        if ($colCount -ne 5) {
            Write-Host "BẢNG $i CÓ $colCount CỘT (KHÁC 5)!"
            $non5ColTables2++
        }
    }
}
Write-Host "Bảng test case không đúng 5 cột: $non5ColTables2"
$doc2.Close($false)

$word.Quit()
[System.Runtime.InteropServices.Marshal]::ReleaseComObject($word) | Out-Null
Write-Host "`n=== KIỂM TRA HOÀN TẤT ==="

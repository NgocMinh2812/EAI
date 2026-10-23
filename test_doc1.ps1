$w = New-Object -ComObject Word.Application
$w.Visible = $false
$w.DisplayAlerts = 0

$docPath = "d:\TDC\HK5\CDPTUD\EAI\TestCases_Sprint1_Sprint2_Minh_HocVien.docx"
$d = $w.Documents.Open($docPath)
$pages = $d.ComputeStatistics(2)
$tables = $d.Tables.Count

Write-Host "File 1: $docPath"
Write-Host "Pages: $pages"
Write-Host "Tables: $tables"

$non5 = 0
for ($i = 1; $i -le $tables; $i++) {
    $t = $d.Tables.Item($i)
    $text = $t.Rows.Item(1).Range.Text
    if ($text.Contains("Mã TC")) {
        if ($t.Columns.Count -ne 5) {
            Write-Host "Table $i has $($t.Columns.Count) columns (NOT 5)"
            $non5++
        }
    }
}
Write-Host "Non-5-column test case tables: $non5"

$d.Close($false)
$w.Quit()
[System.Runtime.InteropServices.Marshal]::ReleaseComObject($w) | Out-Null

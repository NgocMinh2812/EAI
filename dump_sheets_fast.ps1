$excel = New-Object -ComObject Excel.Application
$excel.Visible = $false
$excel.DisplayAlerts = $false

$outDir = "d:\TDC\HK5\CDPTUD\EAI\excel_dumps"
if (!(Test-Path $outDir)) { New-Item -ItemType Directory -Path $outDir | Out-Null }

try {
    # File 1: Kehoachtrienkhai.xlsx
    $wb1 = $excel.Workbooks.Open("D:\TDC\HK5\CDPTUD\Kehoachtrienkhai.xlsx")
    for ($i = 1; $i -le $wb1.Sheets.Count; $i++) {
        $sheet = $wb1.Sheets.Item($i)
        $safeName = $sheet.Name -replace '[\\/*?:\[\]]', '_'
        $csvPath = "$outDir\Kehoach_$safeName.csv"
        # 62 = xlCSVUTF8
        $sheet.SaveAs($csvPath, 62)
        Write-Output "Exported: Kehoach_$safeName.csv"
    }
    $wb1.Close($false)

    # File 2: EnglishEAI_BoKiemThu_TestCase.xlsx
    $wb2 = $excel.Workbooks.Open("D:\TDC\HK5\CDPTUD\EAI\EnglishEAI_BoKiemThu_TestCase.xlsx")
    for ($i = 1; $i -le $wb2.Sheets.Count; $i++) {
        $sheet = $wb2.Sheets.Item($i)
        $safeName = $sheet.Name -replace '[\\/*?:\[\]]', '_'
        $csvPath = "$outDir\TestCasesExisting_$safeName.csv"
        $sheet.SaveAs($csvPath, 62)
        Write-Output "Exported: TestCasesExisting_$safeName.csv"
    }
    $wb2.Close($false)
}
finally {
    $excel.Quit()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($excel) | Out-Null
    Get-Process -Name EXCEL -ErrorAction SilentlyContinue | Stop-Process -Force
}

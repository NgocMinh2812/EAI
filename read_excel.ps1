$excel = New-Object -ComObject Excel.Application
$excel.Visible = $false
$excel.DisplayAlerts = $false

try {
    $filePath1 = 'D:\TDC\HK5\CDPTUD\Kehoachtrienkhai.xlsx'
    $wb = $excel.Workbooks.Open($filePath1)
    Write-Output "=== Kehoachtrienkhai.xlsx Sheets ==="
    foreach ($sheet in $wb.Sheets) {
        Write-Output $sheet.Name
    }
    $wb.Close($false)

    $filePath2 = 'D:\TDC\HK5\CDPTUD\EAI\EnglishEAI_BoKiemThu_TestCase.xlsx'
    if (Test-Path $filePath2) {
        $wb2 = $excel.Workbooks.Open($filePath2)
        Write-Output "=== EnglishEAI_BoKiemThu_TestCase.xlsx Sheets ==="
        foreach ($sheet in $wb2.Sheets) {
            Write-Output $sheet.Name
        }
        $wb2.Close($false)
    }
}
finally {
    $excel.Quit()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($excel) | Out-Null
}

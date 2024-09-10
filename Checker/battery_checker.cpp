#include <iostream>
#include <Windows.h>
#include <thread>

void CheckBatteryStatus()
{
    SYSTEM_POWER_STATUS powerStatus;
    if (GetSystemPowerStatus(&powerStatus))
    {
        int batteryPercentage = static_cast<int>(powerStatus.BatteryLifePercent);

        if (batteryPercentage == 255)
        {
            std::wstring unknownMessage = L"Battery percentage is unknown.";
            MessageBoxW(NULL, unknownMessage.c_str(), L"Battery Status", MB_OK | MB_ICONINFORMATION);
            exit(0);
        }

        if (batteryPercentage <= 20 && powerStatus.ACLineStatus != 1)
        {
            std::wstring warningMessage = L"Battery low, please charge. Battery: " + std::to_wstring(batteryPercentage) + L"%";
            MessageBoxW(NULL, warningMessage.c_str(), L"Battery Warning", MB_OK | MB_ICONEXCLAMATION);
        }
        if (batteryPercentage >= 80 && powerStatus.ACLineStatus == 1)
        {
            std::wstring statusMessage = L"Battery perfectly charged, remove the charger. Battery: " + std::to_wstring(batteryPercentage) + L"%";
            MessageBoxW(NULL, statusMessage.c_str(), L"Battery Status", MB_OK | MB_ICONINFORMATION);
        }
    }
}

int main()
{
    while (true)
    {
        CheckBatteryStatus();
        std::this_thread::sleep_for(std::chrono::minutes(1)); // Sleep for 1 minute before checking again
    }

    return 0;
}

SELECT
	hc.Id as Id,
	CASE
		WHEN hc.FirstHealthPriority IS NULL THEN NULL
		WHEN hc.FirstHealthPriority = 'bloodpressure' THEN 'Y'
		ELSE 'N'
	END AS [First Health Priority: Learn what my blood pressure is, and what this means for me],
	CASE
		WHEN hc.FirstHealthPriority IS NULL THEN NULL
		WHEN hc.FirstHealthPriority = 'improvebloodpressure' THEN 'Y'
		ELSE 'N'
	END AS [First Health Priority: Improve my blood pressure],
	CASE
		WHEN hc.FirstHealthPriority IS NULL THEN NULL
		WHEN hc.FirstHealthPriority = 'bloodsugar' THEN 'Y'
		ELSE 'N'
	END AS [First Health Priority: Learn what my blood sugar level is, and what this means for me],
	CASE
		WHEN hc.FirstHealthPriority IS NULL THEN NULL
		WHEN hc.FirstHealthPriority = 'improvebloodsugar' THEN 'Y'
		ELSE 'N'
	END AS [First Health Priority: Improve my blood sugar level],
	CASE
		WHEN hc.FirstHealthPriority IS NULL THEN NULL
		WHEN hc.FirstHealthPriority = 'smoking' THEN 'Y'
		ELSE 'N'
	END AS [First Health Priority: Stop smoking],
	CASE
		WHEN hc.FirstHealthPriority IS NULL THEN NULL
		WHEN hc.FirstHealthPriority = 'alcohol' THEN 'Y'
		ELSE 'N'
	END AS [First Health Priority: Drink less alcohol],
	CASE
		WHEN hc.FirstHealthPriority IS NULL THEN NULL
		WHEN hc.FirstHealthPriority = 'weight' THEN 'Y'
		ELSE 'N'
	END AS [First Health Priority: Healthy weight],
	CASE
		WHEN hc.FirstHealthPriority IS NULL THEN NULL
		WHEN hc.FirstHealthPriority = 'cholesterol' THEN 'Y'
		ELSE 'N'
	END AS [First Health Priority: Learn what my cholesterol levels are, and what this means for me],
	CASE
		WHEN hc.FirstHealthPriority IS NULL THEN NULL
		WHEN hc.FirstHealthPriority = 'improvecholesterol' THEN 'Y'
		ELSE 'N'
	END AS [First Health Priority: Improve my cholesterol levels],
	CASE
		WHEN hc.FirstHealthPriority IS NULL THEN NULL
		WHEN hc.FirstHealthPriority = 'move' THEN 'Y'
		ELSE 'N'
	END AS [First Health Priority: Move more],
	CASE
		WHEN hc.FirstHealthPriority IS NULL THEN NULL
		WHEN hc.FirstHealthPriority = 'mental' THEN 'Y'
		ELSE 'N'
	END AS [First Health Priority: Mental wellbeing],
	CASE
		WHEN hc.SecondHealthPriority IS NULL THEN NULL
		WHEN hc.SecondHealthPriority = 'bloodpressure' THEN 'Y'
		ELSE 'N'
	END AS [Second Health Priority: Learn what my blood pressure is, and what this means for me],
	CASE
		WHEN hc.SecondHealthPriority IS NULL THEN NULL
		WHEN hc.SecondHealthPriority = 'improvebloodpressure' THEN 'Y'
		ELSE 'N'
	END AS [Second Health Priority: Improve my blood pressure],
	CASE
		WHEN hc.SecondHealthPriority IS NULL THEN NULL
		WHEN hc.SecondHealthPriority = 'bloodsugar' THEN 'Y'
		ELSE 'N'
	END AS [Second Health Priority: Learn what my blood sugar level is, and what this means for me],
	CASE
		WHEN hc.SecondHealthPriority IS NULL THEN NULL
		WHEN hc.SecondHealthPriority = 'improvebloodsugar' THEN 'Y'
		ELSE 'N'
	END AS [Second Health Priority: Improve my blood sugar level],
	CASE
		WHEN hc.SecondHealthPriority IS NULL THEN NULL
		WHEN hc.SecondHealthPriority = 'smoking' THEN 'Y'
		ELSE 'N'
	END AS [Second Health Priority: Stop smoking],
	CASE
		WHEN hc.SecondHealthPriority IS NULL THEN NULL
		WHEN hc.SecondHealthPriority = 'alcohol' THEN 'Y'
		ELSE 'N'
	END AS [Second Health Priority: Drink less alcohol],
	CASE
		WHEN hc.SecondHealthPriority IS NULL THEN NULL
		WHEN hc.SecondHealthPriority = 'weight' THEN 'Y'
		ELSE 'N'
	END AS [Second Health Priority: Healthy weight],
	CASE
		WHEN hc.SecondHealthPriority IS NULL THEN NULL
		WHEN hc.SecondHealthPriority = 'cholesterol' THEN 'Y'
		ELSE 'N'
	END AS [Second Health Priority: Learn what my cholesterol levels are, and what this means for me],
	CASE
		WHEN hc.SecondHealthPriority IS NULL THEN NULL
		WHEN hc.SecondHealthPriority = 'improvecholesterol' THEN 'Y'
		ELSE 'N'
	END AS [Second Health Priority: Improve my cholesterol levels],
	CASE
		WHEN hc.SecondHealthPriority IS NULL THEN NULL
		WHEN hc.SecondHealthPriority = 'move' THEN 'Y'
		ELSE 'N'
	END AS [Second Health Priority: Move more],
	CASE
		WHEN hc.SecondHealthPriority IS NULL THEN NULL
		WHEN hc.SecondHealthPriority = 'mental' THEN 'Y'
		ELSE 'N'
	END AS [Second Health Priority: Mental wellbeing],
	CASE
		WHEN hc.EasyToChange IS NULL THEN NULL
		ELSE hc.EasyToChange
	END AS [I find it easy to change where my health is concerned],
	CASE
		WHEN hc.MaintainChange IS NULL THEN NULL
		ELSE hc.MaintainChange
	END AS [I find it easy to maintain changes where my health is concerned],
	CASE
		WHEN hc.Height IS NULL THEN 'N'
		ELSE 'Y'
	END AS [Height and weight known],
	CASE
		WHEN hc.SexAtBirth IS NULL THEN NULL
		WHEN hc.SexAtBirth = 0 THEN 'Y'
		ELSE 'N'
	END AS [Sex assigned at birth: Female],
	CASE
		WHEN hc.SexAtBirth IS NULL THEN NULL
		WHEN hc.SexAtBirth = 1 THEN 'Y'
		ELSE 'N'
	END AS [Sex assigned at birth: Male],
	CASE
		WHEN hc.Ethnicity IS NULL THEN NULL
		WHEN hc.Ethnicity IN (0,1,2,3) THEN 'Y'
		ELSE 'N'
	END AS [Ethnicity: White],
	CASE
		WHEN hc.Ethnicity IS NULL THEN NULL
		WHEN hc.Ethnicity IN (4,5,6,7) THEN 'Y'
		ELSE 'N'
	END AS [Ethnicity: Mixed],
	CASE
		WHEN hc.Ethnicity IS NULL THEN NULL
		WHEN hc.Ethnicity IN (8,9,10,11,12) THEN 'Y'
		ELSE 'N'
	END AS [Ethnicity: Asian],
	CASE
		WHEN hc.Ethnicity IS NULL THEN NULL
		WHEN hc.Ethnicity IN (13,14,15) THEN 'Y'
		ELSE 'N'
	END AS [Ethnicity: Black],
	CASE
		WHEN hc.Ethnicity IS NULL THEN NULL
		WHEN hc.Ethnicity IN (16,17) THEN 'Y'
		ELSE 'N'
	END AS [Ethnicity: Other],
	CASE
		WHEN hc.Age IS NULL THEN NULL
		WHEN hc.Age BETWEEN 40 AND 49 THEN 'Y'
		ELSE 'N'
	END AS [Age: 40-49 years],
	CASE
		WHEN hc.Age IS NULL THEN NULL
		WHEN hc.Age BETWEEN 50 AND 59 THEN 'Y'
		ELSE 'N'
	END AS [Age: 50-59 years],
	CASE
		WHEN hc.Age IS NULL THEN NULL
		WHEN hc.Age BETWEEN 60 AND 69 THEN 'Y'
		ELSE 'N'
	END AS [Age: 60-69 years],
	CASE
		WHEN hc.Age IS NULL THEN NULL
		WHEN hc.Age BETWEEN 70 AND 79 THEN 'Y'
		ELSE 'N'
	END AS [Age: 70-79 years],
	CASE
		WHEN hc.BloodSugar IS NULL THEN 'N'
		ELSE 'Y'
	END AS [Blood sugar measurement entered],
	CASE
		WHEN hc.SystolicBloodPressure IS NULL THEN 'N'
		ELSE 'Y'
	END AS [Blood pressure measurement entered],
	CASE
		WHEN hc.TotalCholesterol IS NULL THEN 'N'
		ELSE 'Y'
	END AS [Cholesterol measurement entered],
	CASE
		WHEN hc.EmailAddress IS NULL THEN 'N'
		ELSE 'Y'
	END AS [Follow up and notification: email provided],
	CASE
		WHEN hc.SmsNumber IS NULL THEN 'N'
		ELSE 'Y'
	END AS [Follow up and notification: SMS provided],
	CASE
		WHEN hc.PhoneNumber IS NULL THEN 'N'
		ELSE 'Y'
	END AS [Follow up and notification: phone provided],
	CASE
		WHEN hc.Postcode IS NULL THEN 'N'
		WHEN hc.Postcode IS NOT NULL AND hc.ValidationOverwritten = 1 THEN 'N'
		ELSE 'Y'
	END AS [Validated],
	CASE
		WHEN hc.GPSurgery IS NULL THEN NULL
		WHEN LOWER(hc.GPSurgery) = 'the forest hill road group practice' THEN 'Y'
		ELSE 'N'
	END AS [Patient at Forest Hill Practice],
	CASE
		WHEN hc.GPSurgery IS NULL THEN NULL
		WHEN LOWER(hc.GPSurgery) = 'dmc healthcare chadwick road' THEN 'Y'
		ELSE 'N'
	END AS [Patient at Chadwick Road Practice],
	CASE
		WHEN hc.GPSurgery IS NULL THEN NULL
		WHEN LOWER(hc.GPSurgery) = 'the dulwich medical centre' THEN 'Y'
		ELSE 'N'
	END AS [Patient at Dulwich Medical Centre Practice],
	CASE
		WHEN hc.GPSurgery IS NULL THEN NULL
		WHEN LOWER(hc.GPSurgery) = 'sternhall lane surgery' THEN 'Y'
		ELSE 'N'
	END AS [Patient at Sternhall Lane Practice],
	CASE
		WHEN hc.QRISK IS NULL THEN NULL
		WHEN hc.QRISK < 10 THEN 'Y'
		ELSE 'N'
	END AS [QRISK3 (CVD): Low],
	CASE
		WHEN hc.QRISK IS NULL THEN NULL
		WHEN hc.QRISK < 20 AND hc.QRISK >= 10 THEN 'Y'
		ELSE 'N'
	END AS [QRISK3 (CVD): Moderate],
	CASE
		WHEN hc.QRISK IS NULL THEN NULL
		WHEN hc.QRISK < 30 AND hc.QRISK >= 20 THEN 'Y'
		ELSE 'N'
	END AS [QRISK3 (CVD): High],
	CASE
		WHEN hc.QRISK IS NULL THEN NULL
		WHEN hc.QRISK >= 30 THEN 'Y'
		ELSE 'N'
	END AS [QRISK3 (CVD): Very High],
	CASE
		WHEN hc.HeartAge IS NULL THEN NULL
		-- Andy's Heart Age Amber Threshold
		WHEN hc.HeartAge > hc.Age + ROUND(((hc.Age - 40) / 5)+5,0) THEN 'N'
		ELSE 'Y'
	END AS [Heart Age: Low],
	CASE
		WHEN hc.HeartAge IS NULL THEN NULL
		-- Andy's Heart Age Amber Threshold
		WHEN hc.HeartAge > hc.Age + ROUND(((hc.Age - 40) / 5)+5,0) THEN 'Y'
		ELSE 'N'
	END AS [Heart Age: High],
	CASE
		WHEN hc.QDiabetes IS NULL THEN NULL
		WHEN hc.QDiabetes < 5.3 THEN 'Y'
		ELSE 'N'
	END AS [QDiabetes: Low],
	CASE
		WHEN hc.QDiabetes IS NULL THEN NULL
		WHEN hc.QDiabetes < 5.3 THEN 'N'
		ELSE 'Y'
	END AS [QDiabetes: High],
	CASE
		WHEN hc.SmokingStatus IS NULL THEN NULL
		WHEN hc.SmokingStatus < 2 THEN 'Y'
		ELSE 'N'
	END AS [Smoking: Low],
	CASE
		WHEN hc.SmokingStatus IS NULL THEN NULL
		WHEN hc.SmokingStatus >= 2 THEN 'Y'
		ELSE 'N'
	END AS [Smoking: High],
	CASE
		WHEN hc.[AUDIT] IS NULL THEN NULL
		WHEN hc.[AUDIT] < 8 THEN 'Y'
		ELSE 'N'
	END AS [AUDIT (Alcohol): Low],
	CASE
		WHEN hc.[AUDIT] IS NULL THEN NULL
		WHEN hc.[AUDIT] >= 8 AND hc.[AUDIT] <= 15 THEN 'Y'
		ELSE 'N'
	END AS [AUDIT (Alcohol): Medium],
	CASE
		WHEN hc.[AUDIT] IS NULL THEN NULL
		WHEN hc.[AUDIT] >= 16 THEN 'Y'
		ELSE 'N'
	END AS [AUDIT (Alcohol): High],
	CASE
		WHEN hc.[GPPAQ] IS NULL THEN NULL
		WHEN hc.[GPPAQ] = 0 THEN 'Y'
		ELSE 'N'
	END AS [GPPAQ (Physical Activity): Inactive],
	CASE
		WHEN hc.[GPPAQ] IS NULL THEN NULL
		WHEN hc.[GPPAQ] = 1 THEN 'Y'
		ELSE 'N'
	END AS [GPPAQ (Physical Activity): Mostly Inactive],
	CASE
		WHEN hc.[GPPAQ] IS NULL THEN NULL
		WHEN hc.[GPPAQ] = 2 THEN 'Y'
		ELSE 'N'
	END AS [GPPAQ (Physical Activity): Mostly Active],
	CASE
		WHEN hc.[GPPAQ] IS NULL THEN NULL
		WHEN hc.[GPPAQ] = 3 THEN 'Y'
		ELSE 'N'
	END AS [GPPAQ (Physical Activity): Active],
	CASE
		WHEN hc.Height IS NULL OR hc.Weight IS NULL THEN NULL
		WHEN (hc.Weight / (hc.Height * hc.Height)) < 16 THEN 'Y'
		ELSE 'N'
	END AS [BMI: Urgently Underweight],
	CASE
		WHEN hc.Height IS NULL OR hc.Weight IS NULL THEN NULL
		WHEN (hc.Weight / (hc.Height * hc.Height)) >= 16 AND (hc.Weight / (hc.Height * hc.Height)) < 18.5 THEN 'Y'
		ELSE 'N'
	END AS [BMI: Underweight],
	CASE
		WHEN hc.Height IS NULL OR hc.Weight IS NULL THEN NULL
		WHEN hc.Ethnicity IN (13,12,10,15,14,11,8,9,5,4,6) AND (hc.Weight / (hc.Height * hc.Height)) >= 18.5 AND (hc.Weight / (hc.Height * hc.Height)) < 23 THEN 'Y'
		WHEN hc.Ethnicity NOT IN (13,12,10,15,14,11,8,9,5,4,6) AND (hc.Weight / (hc.Height * hc.Height)) >= 18.5 AND (hc.Weight / (hc.Height * hc.Height)) < 25 THEN 'Y'
		ELSE 'N'
	END AS [BMI: Healthy],
	CASE
		WHEN hc.Height IS NULL OR hc.Weight IS NULL THEN NULL
		WHEN hc.Ethnicity IN (13,12,10,15,14,11,8,9,5,4,6) AND (hc.Weight / (hc.Height * hc.Height)) >= 23 AND (hc.Weight / (hc.Height * hc.Height)) < 27.5 THEN 'Y'
		WHEN hc.Ethnicity NOT IN (13,12,10,15,14,11,8,9,5,4,6) AND (hc.Weight / (hc.Height * hc.Height)) >= 25 AND (hc.Weight / (hc.Height * hc.Height)) < 30 THEN 'Y'
		ELSE 'N'
	END AS [BMI: Overweight],
	CASE
		WHEN hc.Height IS NULL OR hc.Weight IS NULL THEN NULL
		WHEN hc.Ethnicity IN (13,12,10,15,14,11,8,9,5,4,6) AND (hc.Weight / (hc.Height * hc.Height)) >= 27.5 THEN 'Y'
		WHEN hc.Ethnicity NOT IN (13,12,10,15,14,11,8,9,5,4,6) AND (hc.Weight / (hc.Height * hc.Height)) >= 30 THEN 'Y'
		ELSE 'N'
	END AS [BMI: Obese],
	CASE
		WHEN hc.BloodSugar IS NULL THEN NULL
		WHEN hc.BloodSugar < 42 THEN 'Y'
		ELSE 'N'
	END AS [HbA1c: Low],
	CASE
		WHEN hc.BloodSugar IS NULL THEN NULL
		WHEN hc.BloodSugar >= 42 AND hc.BloodSugar < 47 THEN 'Y'
		ELSE 'N'
	END AS [HbA1c: Medium],
	CASE
		WHEN hc.BloodSugar IS NULL THEN NULL
		WHEN hc.BloodSugar >= 47 AND hc.BloodSugar < 100 THEN 'Y'
		ELSE 'N'
	END AS [HbA1c: High],
	CASE
		WHEN hc.BloodSugar IS NULL THEN NULL
		WHEN hc.BloodSugar >= 100 THEN 'Y'
		ELSE 'N'
	END AS [HbA1c: Severe],
	CASE
		WHEN hc.SystolicBloodPressure IS NULL OR hc.DiastolicBloodPressure IS NULL THEN NULL
		WHEN hc.SystolicBloodPressure < 90 THEN 'Y'
		WHEN hc.DiastolicBloodPressure < 60 THEN 'Y'
		ELSE 'N'
	-- BP is recorded as Low if any is Low, and then is recorded as the highest risk of either Systolic or Diastolic. 
	-- So e.g. if Sys is Healthy but Dias is Slightly High, the result is Slightly High.
	END AS [Blood Pressure: Low],
	CASE
		WHEN hc.SystolicBloodPressure IS NULL OR hc.DiastolicBloodPressure IS NULL THEN NULL
		WHEN hc.SystolicBloodPressure >= 90 AND hc.SystolicBloodPressure < 120 AND hc.DiastolicBloodPressure >= 60 AND hc.DiastolicBloodPressure < 80 THEN 'Y'
		ELSE 'N'
	END AS [Blood Pressure: Healthy],
	CASE
		WHEN hc.SystolicBloodPressure IS NULL OR hc.DiastolicBloodPressure IS NULL THEN NULL
		WHEN hc.SystolicBloodPressure >= 120 AND hc.SystolicBloodPressure < 140 AND hc.DiastolicBloodPressure >= 60 AND hc.DiastolicBloodPressure < 90 THEN 'Y'
		WHEN hc.DiastolicBloodPressure >= 80 AND hc.DiastolicBloodPressure < 90 AND hc.SystolicBloodPressure >= 90 AND hc.SystolicBloodPressure < 140 THEN 'Y'
		ELSE 'N'
	END AS [Blood Pressure: Slightly High],
	CASE
		WHEN hc.SystolicBloodPressure IS NULL OR hc.DiastolicBloodPressure IS NULL THEN NULL
		WHEN hc.SystolicBloodPressure >= 140 AND hc.SystolicBloodPressure < 180 AND hc.DiastolicBloodPressure >= 60 AND hc.DiastolicBloodPressure < 110 THEN 'Y'
		WHEN hc.DiastolicBloodPressure >= 90 AND hc.DiastolicBloodPressure < 110 AND hc.SystolicBloodPressure >= 90 AND hc.SystolicBloodPressure < 180 THEN 'Y'
		ELSE 'N'
	END AS [Blood Pressure: High],
	CASE
		WHEN hc.SystolicBloodPressure IS NULL OR hc.DiastolicBloodPressure IS NULL THEN NULL
		WHEN hc.SystolicBloodPressure >= 180 AND hc.DiastolicBloodPressure >= 60 THEN 'Y'
		WHEN hc.DiastolicBloodPressure >= 110 AND hc.SystolicBloodPressure >= 90 THEN 'Y'
		ELSE 'N'
	END AS [Blood Pressure: Very High],
	CASE
		WHEN hc.TotalCholesterol IS NULL THEN NULL
		WHEN hc.TotalCholesterol <= 5 THEN 'Y'
		ELSE 'N'
	END AS [Cholesterol: Low],
	CASE
		WHEN hc.TotalCholesterol IS NULL THEN NULL
		WHEN hc.TotalCholesterol > 5 AND hc.TotalCholesterol < 7.5 THEN 'Y'
		ELSE 'N'
	END AS [Cholesterol: Medium],
	CASE
		WHEN hc.TotalCholesterol IS NULL THEN NULL
		WHEN hc.TotalCholesterol >= 7.5 THEN 'Y'
		ELSE 'N'
	END AS [Cholesterol: High],
	CASE
		WHEN hc.GAD2 IS NULL THEN NULL
		WHEN hc.GAD2 < 2 AND hc.FeelingDown = 0 AND hc.Disinterested = 0 THEN 'Y'
		ELSE 'N'
	END AS [Mental Wellbeing: Low],
	CASE
		WHEN hc.GAD2 IS NULL THEN NULL
		WHEN (NOT (hc.GAD2 < 2 AND hc.FeelingDown = 0 AND hc.Disinterested = 0) AND  NOT (hc.GAD2 >= 5 AND hc.FeelingDown = 1 AND hc.Disinterested = 1)) THEN 'Y'
		ELSE 'N'
	END AS [Mental Wellbeing: Medium],
	CASE
		WHEN hc.GAD2 IS NULL THEN NULL
		WHEN hc.GAD2 >= 5 AND hc.FeelingDown = 1 AND hc.Disinterested = 1 THEN 'Y'
		ELSE 'N'
	END AS [Mental Wellbeing: High],
	CASE
		WHEN hc.FirstHealthPriorityAfterResults IS NULL THEN NULL
		WHEN hc.FirstHealthPriorityAfterResults = 'bloodpressure' THEN 'Y'
		ELSE 'N'
	END AS [Follow-up First Health Priority: Learn what my blood pressure is, and what this means for me],
	CASE
		WHEN hc.FirstHealthPriorityAfterResults IS NULL THEN NULL
		WHEN hc.FirstHealthPriorityAfterResults = 'improvebloodpressure' THEN 'Y'
		ELSE 'N'
	END AS [Follow-up First Health Priority: Improve my blood pressure],
	CASE
		WHEN hc.FirstHealthPriorityAfterResults IS NULL THEN NULL
		WHEN hc.FirstHealthPriorityAfterResults = 'bloodsugar' THEN 'Y'
		ELSE 'N'
	END AS [Follow-up First Health Priority: Learn what my blood sugar level is, and what this means for me],
	CASE
		WHEN hc.FirstHealthPriorityAfterResults IS NULL THEN NULL
		WHEN hc.FirstHealthPriorityAfterResults = 'improvebloodsugar' THEN 'Y'
		ELSE 'N'
	END AS [Follow-up First Health Priority: Improve my blood sugar level],
	CASE
		WHEN hc.FirstHealthPriorityAfterResults IS NULL THEN NULL
		WHEN hc.FirstHealthPriorityAfterResults = 'smoking' THEN 'Y'
		ELSE 'N'
	END AS [Follow-up First Health Priority: Stop smoking],
	CASE
		WHEN hc.FirstHealthPriorityAfterResults IS NULL THEN NULL
		WHEN hc.FirstHealthPriorityAfterResults = 'alcohol' THEN 'Y'
		ELSE 'N'
	END AS [Follow-up First Health Priority: Drink less alcohol],
	CASE
		WHEN hc.FirstHealthPriorityAfterResults IS NULL THEN NULL
		WHEN hc.FirstHealthPriorityAfterResults = 'weight' THEN 'Y'
		ELSE 'N'
	END AS [Follow-up First Health Priority: Healthy weight],
	CASE
		WHEN hc.FirstHealthPriorityAfterResults IS NULL THEN NULL
		WHEN hc.FirstHealthPriorityAfterResults = 'cholesterol' THEN 'Y'
		ELSE 'N'
	END AS [Follow-up First Health Priority: Learn what my cholesterol levels are, and what this means for me],
	CASE
		WHEN hc.FirstHealthPriorityAfterResults IS NULL THEN NULL
		WHEN hc.FirstHealthPriorityAfterResults = 'improvecholesterol' THEN 'Y'
		ELSE 'N'
	END AS [Follow-up First Health Priority: Improve my cholesterol levels],
	CASE
		WHEN hc.FirstHealthPriorityAfterResults IS NULL THEN NULL
		WHEN hc.FirstHealthPriorityAfterResults = 'move' THEN 'Y'
		ELSE 'N'
	END AS [Follow-up First Health Priority: Move more],
	CASE
		WHEN hc.FirstHealthPriorityAfterResults IS NULL THEN NULL
		WHEN hc.FirstHealthPriorityAfterResults = 'mental' THEN 'Y'
		ELSE 'N'
	END AS [Follow-up First Health Priority: Mental wellbeing],
	CASE
		WHEN hc.SecondHealthPriorityAfterResults IS NULL THEN NULL
		WHEN hc.SecondHealthPriorityAfterResults = 'bloodpressure' THEN 'Y'
		ELSE 'N'
	END AS [Follow-up Second Health Priority: Learn what my blood pressure is, and what this means for me],
	CASE
		WHEN hc.SecondHealthPriorityAfterResults IS NULL THEN NULL
		WHEN hc.SecondHealthPriorityAfterResults = 'improvebloodpressure' THEN 'Y'
		ELSE 'N'
	END AS [Follow-up Second Health Priority: Improve my blood pressure],
	CASE
		WHEN hc.SecondHealthPriorityAfterResults IS NULL THEN NULL
		WHEN hc.SecondHealthPriorityAfterResults = 'bloodsugar' THEN 'Y'
		ELSE 'N'
	END AS [Follow-up Second Health Priority: Learn what my blood sugar level is, and what this means for me],
	CASE
		WHEN hc.SecondHealthPriorityAfterResults IS NULL THEN NULL
		WHEN hc.SecondHealthPriorityAfterResults = 'improvebloodsugar' THEN 'Y'
		ELSE 'N'
	END AS [Follow-up Second Health Priority: Improve my blood sugar level],
	CASE
		WHEN hc.SecondHealthPriorityAfterResults IS NULL THEN NULL
		WHEN hc.SecondHealthPriorityAfterResults = 'smoking' THEN 'Y'
		ELSE 'N'
	END AS [Follow-up Second Health Priority: Stop smoking],
	CASE
		WHEN hc.SecondHealthPriorityAfterResults IS NULL THEN NULL
		WHEN hc.SecondHealthPriorityAfterResults = 'alcohol' THEN 'Y'
		ELSE 'N'
	END AS [Follow-up Second Health Priority: Drink less alcohol],
	CASE
		WHEN hc.SecondHealthPriorityAfterResults IS NULL THEN NULL
		WHEN hc.SecondHealthPriorityAfterResults = 'weight' THEN 'Y'
		ELSE 'N'
	END AS [Follow-up Second Health Priority: Healthy weight],
	CASE
		WHEN hc.SecondHealthPriorityAfterResults IS NULL THEN NULL
		WHEN hc.SecondHealthPriorityAfterResults = 'cholesterol' THEN 'Y'
		ELSE 'N'
	END AS [Follow-up Second Health Priority: Learn what my cholesterol levels are, and what this means for me],
	CASE
		WHEN hc.SecondHealthPriorityAfterResults IS NULL THEN NULL
		WHEN hc.SecondHealthPriorityAfterResults = 'improvecholesterol' THEN 'Y'
		ELSE 'N'
	END AS [Follow-up Second Health Priority: Improve my cholesterol levels],
	CASE
		WHEN hc.SecondHealthPriorityAfterResults IS NULL THEN NULL
		WHEN hc.SecondHealthPriorityAfterResults = 'move' THEN 'Y'
		ELSE 'N'
	END AS [Follow-up Second Health Priority: Move more],
	CASE
		WHEN hc.SecondHealthPriorityAfterResults IS NULL THEN NULL
		WHEN hc.SecondHealthPriorityAfterResults = 'mental' THEN 'Y'
		ELSE 'N'
	END AS [Follow-up Second Health Priority: Mental wellbeing],
	CASE
		WHEN hc.SystolicBloodPressure >= 180 AND hc.DiastolicBloodPressure >= 60 THEN 'Y'
		WHEN hc.DiastolicBloodPressure >= 110 AND hc.SystolicBloodPressure >= 90 THEN 'Y'
		WHEN hc.BloodSugar >= 100 THEN 'Y'
		WHEN hc.QRISK >= 10 THEN 'Y'
		WHEN hc.QDiabetes >= 5.6 THEN 'Y'
		ELSE 'N'
	END AS [Follow-up: Visit Your GP clinic],
	CASE
		WHEN hc.QRISK is NULL THEN NULL
		WHEN (SELECT COUNT(1) FROM dbo.HealthCheckIntervention hci WHERE hci.HealthChecksId = hc.Id AND hci.ChosenInterventionsId IN (11,28,45,56,60,112,124,136)) > 0 THEN 'Y'
		ELSE 'N'
	END AS [User eligible for referral to Everyone Health],
	CASE
		WHEN hc.EveryoneHealthConsent IS NULL THEN NULL
		WHEN hc.EveryoneHealthConsent = 1 THEN 'Y'
		ELSE 'N'
	END AS [User wishes to be contacted by Everyone Health],
	CASE
		WHEN hc.PrefersToContactEveryoneHealth IS NULL THEN NULL
		WHEN hc.PrefersToContactEveryoneHealth = 1 THEN 'Y'
		ELSE 'N'
	END AS [User wants to contact Everyone Health independently],
	CASE
		WHEN hc.FirstHealthPriority IS NULL THEN 'N'
		ELSE 'Y'
	END AS [Initial Health Priorities Made],
	CASE
		WHEN hc.MaintainChange IS NULL THEN 'N'
		ELSE 'Y'
	END AS [Initial Readiness to Change Page Completed],
	CASE
		WHEN hc.Postcode IS NULL THEN 'N'
		ELSE 'Y'
	END AS [Validation],
	CASE
		WHEN hc.QRISK IS NULL THEN 'N'
		ELSE 'Y'
	END AS [ResultsPage],
	CASE
		WHEN hc.FirstHealthPriorityAfterResults = 'bloodpressure' AND hc.BloodPressureFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.FirstHealthPriorityAfterResults = 'improvebloodpressure' AND hc.ImproveBloodPressureFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.FirstHealthPriorityAfterResults = 'bloodsugar' AND hc.BloodSugarFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.FirstHealthPriorityAfterResults = 'improvebloodsugar' AND hc.ImproveBloodSugarFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.FirstHealthPriorityAfterResults = 'smoking' AND hc.QuitSmokingFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.FirstHealthPriorityAfterResults = 'alcohol' AND hc.DrinkingFrequency IS NOT NULL THEN 'Y'
		WHEN hc.FirstHealthPriorityAfterResults = 'weight' AND hc.HealthyWeightFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.FirstHealthPriorityAfterResults = 'cholesterol' AND hc.CholesterolFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.FirstHealthPriorityAfterResults = 'improvecholesterol' AND hc.ImproveCholesterolFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.FirstHealthPriorityAfterResults = 'move' AND hc.MoveMoreFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.FirstHealthPriorityAfterResults = 'mental' AND hc.MentalWellbeingFollowUpId IS NOT NULL THEN 'Y'
		ELSE 'N'
	END AS [User completed follow-up page for first health priority option],
	CASE
		WHEN hc.SecondHealthPriorityAfterResults = 'bloodpressure' AND hc.BloodPressureFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.SecondHealthPriorityAfterResults = 'improvebloodpressure' AND hc.ImproveBloodPressureFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.SecondHealthPriorityAfterResults = 'bloodsugar' AND hc.BloodSugarFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.SecondHealthPriorityAfterResults = 'improvebloodsugar' AND hc.ImproveBloodSugarFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.SecondHealthPriorityAfterResults = 'smoking' AND hc.QuitSmokingFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.SecondHealthPriorityAfterResults = 'alcohol' AND hc.DrinkingFrequency IS NOT NULL THEN 'Y'
		WHEN hc.SecondHealthPriorityAfterResults = 'weight' AND hc.HealthyWeightFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.SecondHealthPriorityAfterResults = 'cholesterol' AND hc.CholesterolFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.SecondHealthPriorityAfterResults = 'improvecholesterol' AND hc.ImproveCholesterolFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.SecondHealthPriorityAfterResults = 'move' AND hc.MoveMoreFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.SecondHealthPriorityAfterResults = 'mental' AND hc.MentalWellbeingFollowUpId IS NOT NULL THEN 'Y'
		ELSE 'N'
	END AS [User completed follow-up page for second health priority option],
	CASE
		WHEN hc.FirstHealthPriorityAfterResults <> 'bloodpressure' AND hc.SecondHealthPriorityAfterResults <> 'bloodpressure' AND hc.BloodPressureFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.FirstHealthPriorityAfterResults <> 'improvebloodpressure' AND hc.SecondHealthPriorityAfterResults <> 'improvebloodpressure' AND hc.ImproveBloodPressureFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.FirstHealthPriorityAfterResults <> 'bloodsugar' AND hc.SecondHealthPriorityAfterResults <> 'bloodsugar' AND hc.BloodSugarFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.FirstHealthPriorityAfterResults <> 'improvebloodsugar' AND hc.SecondHealthPriorityAfterResults <> 'improvebloodsugar' AND hc.ImproveBloodSugarFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.FirstHealthPriorityAfterResults <> 'smoking' AND hc.SecondHealthPriorityAfterResults <> 'smoking' AND hc.QuitSmokingFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.FirstHealthPriorityAfterResults <> 'alcohol' AND hc.SecondHealthPriorityAfterResults <> 'alcohol' AND hc.DrinkingFrequency IS NOT NULL THEN 'Y'
		WHEN hc.FirstHealthPriorityAfterResults <> 'weight' AND hc.SecondHealthPriorityAfterResults <> 'weight' AND hc.HealthyWeightFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.FirstHealthPriorityAfterResults <> 'cholesterol' AND hc.SecondHealthPriorityAfterResults <> 'cholesterol' AND hc.CholesterolFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.FirstHealthPriorityAfterResults <> 'improvecholesterol' AND hc.SecondHealthPriorityAfterResults <> 'improvecholesterol' AND hc.ImproveCholesterolFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.FirstHealthPriorityAfterResults <> 'move' AND hc.SecondHealthPriorityAfterResults <> 'move' AND hc.MoveMoreFollowUpId IS NOT NULL THEN 'Y'
		WHEN hc.FirstHealthPriorityAfterResults <> 'mental' AND hc.SecondHealthPriorityAfterResults <> 'mental' AND hc.MentalWellbeingFollowUpId IS NOT NULL THEN 'Y'
		ELSE 'N'
	END AS [User completed three or more follow-up pages]
-- missing fields so far:
-- Whether Gender Identity is same as sex at birth (not enough data currently to bin this)
-- User completes actions first healthy priority page. We can get if actions have been chosen, but it's possible and valid to choose zero actions.
-- User completes actions second health priority page. Same as above. This is also all shown on the same page, so if one's been done, then all have been done.
-- User completes three or more actions health priority pages. Same as above. This is also all shown on the same page, so if one's been done, then all have been done.
-- Completes Digital NHS Health Check. Needs extra work to track.
-- Re-enters tool to provide H and W. Needs extra work to track.
-- Re-enters tool to provide blood sugar. Needs extra work to track.
-- Re-enters tool to provide blood pressure. Needs extra work to track.
-- Re-enters tool to provide cholesterol. Needs extra work to track.
FROM HealthChecks hc
ORDER BY hc.CalculatedDate DESC
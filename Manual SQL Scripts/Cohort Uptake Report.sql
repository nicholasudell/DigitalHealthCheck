SELECT
	a.Invite AS InviteGroup,
	SUM(OpenedLink) as OpenedLink,
	SUM(AnsweredAQuestion) as AnsweredAQuestion,
	SUM([Validation]) as [Validation],
	SUM(Results) as Results,
	SUM(AtLeast1FollowUp) As AtLeast1FollowUp,
	SUM(AtLeast1Intervention) as AtLeast1Intervention,
	SUM(HealthCheckCompleted) as HealthCheckCompleted
FROM
(
	SELECT 
		tu.Invite,
		1 AS Invited,
		1 AS OpenedLink,
		CASE
    		WHEN hc.FirstHealthPriority IS NOT NULL THEN 1
    		ELSE 0
		END AS AnsweredAQuestion,
		CASE
    		WHEN hc.Postcode IS NOT NULL THEN 1
    		ELSE 0
		END AS [Validation],
		CASE
    		WHEN hc.QRISK IS NOT NULL THEN 1
    		ELSE 0
		END AS Results,
		CASE
    		WHEN hc.FirstHealthPriorityAfterResults = 'bloodpressure' AND hc.BloodPressureFollowUpId IS NOT NULL THEN 1
    		WHEN hc.FirstHealthPriorityAfterResults = 'improvebloodpressure' AND hc.ImproveBloodPressureFollowUpId IS NOT NULL THEN 1
    		WHEN hc.FirstHealthPriorityAfterResults = 'bloodsugar' AND hc.BloodSugarFollowUpId IS NOT NULL THEN 1
    		WHEN hc.FirstHealthPriorityAfterResults = 'improvebloodsugar' AND hc.ImproveBloodSugarFollowUpId IS NOT NULL THEN 1
    		WHEN hc.FirstHealthPriorityAfterResults = 'smoking' AND hc.QuitSmokingFollowUpId IS NOT NULL THEN 1
    		WHEN hc.FirstHealthPriorityAfterResults = 'alcohol' AND hc.DrinkingFrequency IS NOT NULL THEN 1
    		WHEN hc.FirstHealthPriorityAfterResults = 'weight' AND hc.HealthyWeightFollowUpId IS NOT NULL THEN 1
    		WHEN hc.FirstHealthPriorityAfterResults = 'cholesterol' AND hc.CholesterolFollowUpId IS NOT NULL THEN 1
    		WHEN hc.FirstHealthPriorityAfterResults = 'improvecholesterol' AND hc.ImproveCholesterolFollowUpId IS NOT NULL THEN 1
    		WHEN hc.FirstHealthPriorityAfterResults = 'move' AND hc.MoveMoreFollowUpId IS NOT NULL THEN 1
    		WHEN hc.FirstHealthPriorityAfterResults = 'mental' AND hc.MentalWellbeingFollowUpId IS NOT NULL THEN 1
    		ELSE 0
		END AS AtLeast1FollowUp,
		CASE
    		WHEN (SELECT COUNT(1) FROM dbo.HealthCheckIntervention hci WHERE hci.HealthChecksId = hc.Id) > 0 THEN 1
    		ELSE 0
		END AS AtLeast1Intervention,
		CONVERT(int, hc.HealthCheckCompleted) AS HealthCheckCompleted,
		CASE
    		WHEN hc.BloodPressureUpdated = 1 THEN 1
    		WHEN hc.BloodSugarUpdated = 1 THEN 1
    		WHEN hc.CholesterolUpdated = 1 THEN 1
    		WHEN hc.HeightAndWeightUpdated = 1 THEN 1
    		ELSE 0
		END AS UpdatedMeasurements
	FROM dbo.HealthChecks hc
	INNER JOIN TestUptake tu ON tu.Id = hc.Id
) a GROUP BY a.Invite ORDER BY a.Invite

-- Special report for invite time for the second group

SELECT
	CONCAT(a.Invite,':00') AS InviteTime,
	SUM(OpenedLink) as OpenedLink,
	SUM(AnsweredAQuestion) as AnsweredAQuestion,
	SUM([Validation]) as [Validation],
	SUM(Results) as Results,
	SUM(AtLeast1FollowUp) As AtLeast1FollowUp,
	SUM(AtLeast1Intervention) as AtLeast1Intervention,
	SUM(HealthCheckCompleted) as HealthCheckCompleted
FROM
(
	SELECT 
		tu.Invite,
		1 AS Invited,
		1 AS OpenedLink,
		CASE
    		WHEN hc.FirstHealthPriority IS NOT NULL THEN 1
    		ELSE 0
		END AS AnsweredAQuestion,
		CASE
    		WHEN hc.Postcode IS NOT NULL THEN 1
    		ELSE 0
		END AS [Validation],
		CASE
    		WHEN hc.QRISK IS NOT NULL THEN 1
    		ELSE 0
		END AS Results,
		CASE
    		WHEN hc.FirstHealthPriorityAfterResults = 'bloodpressure' AND hc.BloodPressureFollowUpId IS NOT NULL THEN 1
    		WHEN hc.FirstHealthPriorityAfterResults = 'improvebloodpressure' AND hc.ImproveBloodPressureFollowUpId IS NOT NULL THEN 1
    		WHEN hc.FirstHealthPriorityAfterResults = 'bloodsugar' AND hc.BloodSugarFollowUpId IS NOT NULL THEN 1
    		WHEN hc.FirstHealthPriorityAfterResults = 'improvebloodsugar' AND hc.ImproveBloodSugarFollowUpId IS NOT NULL THEN 1
    		WHEN hc.FirstHealthPriorityAfterResults = 'smoking' AND hc.QuitSmokingFollowUpId IS NOT NULL THEN 1
    		WHEN hc.FirstHealthPriorityAfterResults = 'alcohol' AND hc.DrinkingFrequency IS NOT NULL THEN 1
    		WHEN hc.FirstHealthPriorityAfterResults = 'weight' AND hc.HealthyWeightFollowUpId IS NOT NULL THEN 1
    		WHEN hc.FirstHealthPriorityAfterResults = 'cholesterol' AND hc.CholesterolFollowUpId IS NOT NULL THEN 1
    		WHEN hc.FirstHealthPriorityAfterResults = 'improvecholesterol' AND hc.ImproveCholesterolFollowUpId IS NOT NULL THEN 1
    		WHEN hc.FirstHealthPriorityAfterResults = 'move' AND hc.MoveMoreFollowUpId IS NOT NULL THEN 1
    		WHEN hc.FirstHealthPriorityAfterResults = 'mental' AND hc.MentalWellbeingFollowUpId IS NOT NULL THEN 1
    		ELSE 0
		END AS AtLeast1FollowUp,
		CASE
    		WHEN (SELECT COUNT(1) FROM dbo.HealthCheckIntervention hci WHERE hci.HealthChecksId = hc.Id) > 0 THEN 1
    		ELSE 0
		END AS AtLeast1Intervention,
		CONVERT(int, hc.HealthCheckCompleted) AS HealthCheckCompleted,
		CASE
    		WHEN hc.BloodPressureUpdated = 1 THEN 1
    		WHEN hc.BloodSugarUpdated = 1 THEN 1
    		WHEN hc.CholesterolUpdated = 1 THEN 1
    		WHEN hc.HeightAndWeightUpdated = 1 THEN 1
    		ELSE 0
		END AS UpdatedMeasurements
	FROM dbo.HealthChecks hc
	INNER JOIN TestUptake tu ON tu.Id = hc.Id
	WHERE tu.Invite > BETWEEN 10 AND 15 --groups 11,12,13 are 11:00, 12:00, and 13:00 invite times.
) a GROUP BY a.Invite ORDER BY a.Invite
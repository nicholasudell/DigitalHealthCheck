-- Short links that completed a check
SELECT tu.ShortLink FROM dbo.HealthChecks hc
INNER JOIN dbo.TestUptake tu ON hc.Id = tu.Id
WHERE HealthCheckCompleted = 1

-- Short links that clicked the link
SELECT tu.ShortLink FROM dbo.HealthChecks hc
INNER JOIN dbo.TestUptake tu ON hc.Id = tu.Id

-- Short links that did not complete a check
SELECT tu.ShortLink FROM dbo.HealthChecks hc
INNER JOIN dbo.TestUptake tu ON hc.Id = tu.Id
WHERE HealthCheckCompleted = 0

-- Short links that arrived but did not answer any questions
SELECT tu.ShortLink FROM dbo.HealthChecks hc
INNER JOIN dbo.TestUptake tu ON hc.Id = tu.Id
WHERE FirstHealthPriority IS NULL AND Height IS NULL -- in older checks, FirstHealthPriority is the first question, in newer checks, it's height and weight.
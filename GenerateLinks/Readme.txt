This console application has three arguments, only the first is required:

1. Path to a CSV file of patient data. Check TestPatientCohort.csv for an example format
2. The format of the link. Set this to a format string with two inputs that specifies the URL format to generate. *{0}* sets the patient's unique id, and *{1}* is the patient's encrypted hash. Typically you will set it to something like `https://<digital-health-check-url>/?id={0}&amp;hash={1}`
3. The path to the output file. This defaults to "links.txt"

An example for running it

./GenerateLinks/GenerateLinks.exe ./TestPatientCohort.csv "https://URL?id={0}&hash={1}"

After running the command, links.txt will contain one link per line.
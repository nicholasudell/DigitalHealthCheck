describe("cholesterol page", () => {
    const pageToTest = '<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Cholesterol';

    beforeEach(async () => {
        await jestPlaywright.resetPage();
        await page.goto(pageToTest);
    })

    it("should show a heading", async () => {
        // after by using expect-playwright
        await expect(page).toMatchText("h1", "Cholesterol");

    })

    it("has expandable info what is cholesterol", async () => {
        const visible = await page.isVisible('text=Cholesterol is a fatty substance found in your blood. It’s created naturally by your liver, but can also be found in certain foods. Your body needs a certain amount of cholesterol to work properly, but too much can be dangerous.');
        expect(visible).toBeFalsy();
        // Click text=What is cholesterol and why does it matter?
        await page.click('text=What is cholesterol and why does it matter?');
        const visibleyes = await page.isVisible('text=Cholesterol is a fatty substance found in your blood. It’s created naturally by your liver, but can also be found in certain foods. Your body needs a certain amount of cholesterol to work properly, but too much can be dangerous.');
        expect(visibleyes).toBeTruthy();   

    })

    it("should not go onto next page if no option selected", async () => {
        // Click text=Continue
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Cholesterol')).toBeTruthy();
    })

    it("has total and hdl fields if yes selected", async () => {
        // Click yes
        await page.click('#know-your-cholesterol', 'yes');
        const checked = await page.isChecked('#know-your-cholesterol', 'yes');
        expect(checked).toBeTruthy();
        // Fill input[name="TotalCholesterolMmoll"]
        await page.fill('input[name="TotalCholesterolMmoll"]', '5');

    })

    it("goes to mental wellbeing when yes selected and cholesterol entered", async () => {
        // Click yes
        await page.click('#know-your-cholesterol', 'yes');
        // Fill input[name="TotalCholesterolMmoll"]
        await page.fill('input[name="TotalCholesterolMmoll"]', '3');
        // Fill input[name="HdlCholesterolMmoll"]
        await page.fill('input[name="HdlCholesterolMmoll"]', '4');
        // Click text=Continue
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/MentalWellbeing')).toBeTruthy();
        
    })

    it("does not require cholesterol input if no selected", async () => {
        // Click no
        await page.click('#know-your-cholesterol-3', 'no');
        const checked = await page.isVisible('input[name="TotalCholesterolMmoll"]');
        expect(checked).toBeFalsy();
        // Click text=Continue
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/MentalWellbeing')).toBeTruthy();
    })

    it("should have a back link", async () => {
        // Click text=Back
        await page.click('text=Back');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/BloodPressure')).toBeTruthy();
      })

  })  

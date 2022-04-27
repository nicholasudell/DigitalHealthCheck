describe("Blood sugar page", () => {
    const pageToTest = '<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/BloodSugar';

    beforeEach(async () => {
        await jestPlaywright.resetPage();
        await page.goto(pageToTest);
    })

    it("should show a heading", async () => {
        // after by using expect-playwright
       await expect(page).toMatchText("h1", "Diabetes page 3 of 3");
    })

    it("should not go onto next page if no option selected", async () => {
        // Click text=Continue
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/BloodSugar')).toBeTruthy();
    })

    it("has HbA1c field if yes selected", async () => {
        // Click yes
        await page.click('#know-your-hba1c', 'yes');
        const checked = await page.isChecked('#know-your-hba1c', 'yes');
        expect(checked).toBeTruthy();
        // Fill input[name="HbA1c"]
        await page.fill('input[name="HbA1c"]', '43');

    })

    it("goes to risk factors 1 when yes selected and bp entered", async () => {
        // Click yes
        await page.click('#know-your-hba1c', 'yes');
        // Fill input[name="HbA1c"]
        await page.fill('input[name="HbA1c"]', '40');
        // Click text=Continue
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/RiskFactors1')).toBeTruthy();
        
    })

    it("does not require bp input if no selected", async () => {
        // Click no
        await page.click('#know-your-hba1c-3', 'no');
        const checked = await page.isVisible('input[name="HbA1c"]');
        expect(checked).toBeFalsy();
        // Click text=Continue
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/RiskFactors1')).toBeTruthy();
    })

    it("should have a back link", async () => {
        // Click text=Back
        await page.click('text=Back');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Diabetes')).toBeTruthy();
      })

  })  

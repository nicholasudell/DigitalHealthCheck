describe("Sex page", () => {
    const pageToTest = '<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Sex';

    beforeEach(async () => {
        await jestPlaywright.resetPage();
        await page.goto(pageToTest);
    })

    it("should show a heading", async () => {
        // after by using expect-playwright
        await expect(page).toMatchText("h1", "Gender identity and sex assigned at birth");
    })

    it("should not go onto next page if no option selected", async () => {
        // Click text=Continue
        await page.click('button:has-text("Continue")');
        expect(page.url()).toBe('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Sex');
    })

    it("selects female cisgender", async () => {
        // Click input[name="Sex"]
        await page.click('input[name="Sex"]', 'female');
        const checked = await page.isChecked('input[name="Sex"]', 'female');
        expect(checked).toBeTruthy();
        await page.click('#identity', 'cis'); 
        await page.click('button:has-text("Continue")');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Ethnicity')).toBeTruthy();
    })

    it("selects male cisgender", async () => {
        // Click input[name="Sex"]
        await page.click('input[name="Sex"]', 'male');
        await page.click('#identity', 'cis'); 
        await page.click('button:has-text("Continue")');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Ethnicity')).toBeTruthy();
    })
    
    it("should have a back link", async () => {
        // Click text=Back
        await page.click('text=Back');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/HeightAndWeight')).toBeTruthy();
    })

    it("goes to gender affirmation if trans woman selected", async () => {
        // Click input[name="Sex"]
        await page.click('input[name="Sex"]', 'male');
        await page.click('#identity-2', 'transwoman'); 
        await page.click('button:has-text("Continue")');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/GenderAffirmation')).toBeTruthy();
    })

    it("goes to gender affirmation if trans man selected", async () => {
        // Click input[name="Sex"]
        await page.click('input[name="Sex"]', 'female');
        await page.click('#identity-3', 'transman'); 
        await page.click('button:has-text("Continue")');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/GenderAffirmation')).toBeTruthy();
    })

    it("goes to gender affirmation if non binary selected", async () => {
        // Click input[name="Sex"]
        await page.click('input[name="Sex"]', 'male');
        await page.click('#identity-4', 'nonbinary'); 
        await page.click('button:has-text("Continue")');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/GenderAffirmation')).toBeTruthy();
    })
})
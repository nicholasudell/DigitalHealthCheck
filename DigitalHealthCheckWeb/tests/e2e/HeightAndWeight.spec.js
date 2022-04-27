describe("Height and Weight page", () => {
    const pageToTest = '<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/HeightAndWeight';

    beforeEach(async () => {
        await jestPlaywright.resetPage();
        await page.goto(pageToTest);
    })

    it("should show a heading", async () => {
        // after by using expect-playwright
        await expect(page).toMatchText("h1", "Height and weight");
    })

    it("should not go onto next page if no values entered", async () => {
        // Click text=Continue
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/HeightAndWeight')).toBeTruthy();
        const error = await page.textContent('#error-summary-title');
        expect(error).toBe('There is a problem');
    })

    it("should allow measurements to be entered in imperial", async () => {
        // Fill input[name="ImperialFeet"]
        await page.fill('input[name="ImperialFeet"]', '5');
        // Fill input[name="ImperialInches"]
        await page.fill('input[name="ImperialInches"]', '6');
        // Fill input[name="ImperialStone"]
        await page.fill('input[name="ImperialStone"]', '11');
        // Fill input[name="ImperialPounds"]
        await page.fill('input[name="ImperialPounds"]', '5');
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Sex')).toBeTruthy();
    })

it("should allow measurements to be entered in metric ", async () => {
         // Click text=Change units to centimetres and kilograms
        await page.click('text=Change units to centimetres and kilograms');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/HeightAndWeight')).toBeTruthy();
        // Fill input[name="MetricHeight"]
        await page.fill('input[name="MetricHeight"]', '160');
        // Fill input[name="MetricWeight"]
        await page.fill('input[name="MetricWeight"]', '70');
        // Click text=Continue
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Sex')).toBeTruthy();
    })

    it("should have a back link", async () => {
        // Click text=Back
        await page.click('text=Back');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/')).toBeTruthy();
    })
})
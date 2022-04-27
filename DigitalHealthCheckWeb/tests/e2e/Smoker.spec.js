describe("Smoker page", () => {
    const pageToTest = '<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Smoker';

    beforeEach(async () => {
        await jestPlaywright.resetPage();
        await page.goto(pageToTest);
    })

    it("should show a heading", async () => {
        // after by using expect-playwright
        await expect(page).toMatchText("h1", "Smoking page 2 of 2");
    })

    it("should not go onto next page if no option selected", async () => {
        // Click text=Continue
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Smoker')).toBeTruthy();
        const error = await page.textContent('#error-summary-title');
        expect(error).toBe('There is a problem');
    })

    it("should select Light smoker (fewer than 10 cigarettes a day) ", async () => {
        // Click Light smoker (fewer than 10 cigarettes a day)
        await page.click('#how-much', 'light');
        const checked = await page.isChecked('#how-much', 'light');
        expect(checked).toBeTruthy();
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/DoYouDrinkAlcohol')).toBeTruthy();
        await expect(page).toMatchText("h1", "Alcohol");
 
    })

    it("should select Moderate smoker (between 10 and 20 cigarettes a day) ", async () => {
        // Moderate smoker (between 10 and 20 cigarettes a day)
        await page.click('#how-much-2', 'moderate');
        const checked = await page.isChecked('#how-much-2', 'moderate');
        expect(checked).toBeTruthy();
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/DoYouDrinkAlcohol')).toBeTruthy();
        await expect(page).toMatchText("h1", "Alcohol");
 
    })

    it("should select Heavy smoker (more than 20 cigarettes a day) ", async () => {
        // Click Heavy smoker (more than 20 cigarettes a day)
        await page.click('#how-much-3', 'heavy');
        const checked = await page.isChecked('#how-much-3', 'heavy');
        expect(checked).toBeTruthy();
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/DoYouDrinkAlcohol')).toBeTruthy();
        await expect(page).toMatchText("h1", "Alcohol");
 
    })

    it("should have a back link", async () => {
        // Click text=Back
        await page.click('text=Back');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Smoking')).toBeTruthy();
    })

})
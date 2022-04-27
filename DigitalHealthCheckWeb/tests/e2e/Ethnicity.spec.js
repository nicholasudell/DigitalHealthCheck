describe("Ethnicity page", () => {
    const pageToTest = '<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Ethnicity';

    beforeEach(async () => {
        await jestPlaywright.resetPage();
        await page.goto(pageToTest);
    })

    it("should show a heading", async () => {
        // after by using expect-playwright
        await expect(page).toMatchText("h1", "Ethnicity");
    })

    it("should not go onto next page if no option selected", async () => {
        // Click text=Continue
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Ethnicity')).toBeTruthy();
        await expect(page).toMatchText("h1", "Ethnicity");
        const error = await page.textContent('#error-summary-title');
        expect(error).toBe('There is a problem');
    })

    it("should select white, then British ", async () => {
        // Click #EthnicGroup-3
        await page.click('#ethnic-group');
        const checked = await page.isChecked('#ethnic-group', 'white');
        expect(checked).toBeTruthy();
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Ethnicity')).toBeTruthy();
        await expect(page).toMatchText("h1", "Which of the following best describes your White background?");
        // Click #Ethnicity
        await page.click('#ethnicity', 'whitebritish');
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Smoking')).toBeTruthy();
    })

    it("should select mixed or multiple ethnic groups, then white and black African ", async () => {
        // Click #EthnicGroup-3
        await page.click('text=Mixed or multiple ethnic groups');
        const checked = await page.isChecked('#ethnic-group-2', 'mixed');
        expect(checked).toBeTruthy();
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Ethnicity')).toBeTruthy();
        await expect(page).toMatchText("h1", "Which of the following best describes your Mixed or Multiple ethnic groups background?");
        await page.click('#ethnicity-2', 'whiteblackafrican');
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Smoking')).toBeTruthy();
    })

    it("should select Asian, then Bangladeshi ", async () => {
        // Click #EthnicGroup-3
        await page.click('#ethnic-group-3', 'asian');
        const checked = await page.isChecked('#ethnic-group-3', 'asian');
        expect(checked).toBeTruthy();
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Ethnicity')).toBeTruthy();
        await expect(page).toMatchText("h1", "Which of the following best describes your Asian or Asian British background?");
        await page.click('#ethnicity-3', 'bangladeshi');
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Smoking')).toBeTruthy();
    })

    it("should select black, then Caribbean", async () => {
        // Click #EthnicGroup-4
        await page.click('#ethnic-group-4', 'black');
        const checked = await page.isChecked('#ethnic-group-4', 'black');
        expect(checked).toBeTruthy();
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Ethnicity')).toBeTruthy();
        await expect(page).toMatchText("h1", "Which of the following best describes your Black, African, Caribbean or Black British background?");
        await page.click('#ethnicity-2', 'caribbean');
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Smoking')).toBeTruthy();
    })

    it("should select other ethnic group, then Arab", async () => {
        // Click #EthnicGroup-5
        await page.click('#ethnic-group-5', 'other');
        const checked = await page.isChecked('#ethnic-group-5', 'other');
        expect(checked).toBeTruthy();
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Ethnicity')).toBeTruthy();
        await expect(page).toMatchText("h1", "Which of the following best describes your background?");
        await page.click('#ethnicity', 'arab');
        await page.click('text=Continue');
        await expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Smoking')).toBeTruthy();
    })

    it("should have a back link", async () => {
        // Click text=Back
        await page.click('text=Back');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Sex')).toBeTruthy();
    })
})
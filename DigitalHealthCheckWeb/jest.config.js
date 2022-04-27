module.exports = {
    rootDir: '.',
    testTimeout: 60000,
    testMatch: [
        '<rootDir>/**/tests/e2e/*.spec.js'
    ],
    preset: 'jest-playwright-preset',
    setupFilesAfterEnv: ["expect-playwright"],
    reporters: ["default", "jest-teamcity"],
    testEnvironment: "<rootDir>/tests/e2e/screenshotreporter.js"
}